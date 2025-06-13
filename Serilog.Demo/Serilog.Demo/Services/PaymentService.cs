using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IOrderService _orderService;
        private static readonly List<Payment> _payments = new();
        private static int _nextPaymentId = 1;

        public PaymentService(ILogger<PaymentService> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "ProcessPayment"))
            using (LogContext.PushProperty("OrderId", request.OrderId))
            using (LogContext.PushProperty("AuditEvent", "PaymentAttempt"))
            {
                _logger.LogInformation("Processing payment for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}", 
                    request.OrderId, request.Amount, request.Method);

                try
                {
                    // Validate order exists
                    var order = await _orderService.GetOrderByIdAsync(request.OrderId);
                    if (order == null)
                    {
                        _logger.LogWarning("Payment failed - Order {OrderId} not found", request.OrderId);
                        throw new InvalidOperationException("Order not found");
                    }

                    // Validate amount matches order total
                    if (request.Amount != order.TotalAmount)
                    {
                        _logger.LogWarning("Payment failed - Amount mismatch for order {OrderId}. Expected: {ExpectedAmount:C}, Provided: {ProvidedAmount:C}", 
                            request.OrderId, order.TotalAmount, request.Amount);
                        throw new ArgumentException("Payment amount does not match order total");
                    }

                    // Simulate payment processing
                    await Task.Delay(500);

                    var payment = new Payment
                    {
                        PaymentId = _nextPaymentId++,
                        OrderId = request.OrderId,
                        Amount = request.Amount,
                        Method = request.Method,
                        Status = PaymentStatus.Pending,
                        ProcessedAt = DateTime.UtcNow
                    };

                    // Simulate payment gateway interaction
                    var success = await SimulatePaymentGatewayAsync(payment);

                    if (success)
                    {
                        payment.Status = PaymentStatus.Completed;
                        payment.TransactionId = $"TXN_{Guid.NewGuid().ToString("N")[..10].ToUpper()}";

                        // Update order status
                        await _orderService.UpdateOrderStatusAsync(request.OrderId, OrderStatus.Processing);
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Failed;
                        payment.FailureReason = "Payment gateway declined the transaction";
                    }

                    _payments.Add(payment);
                    stopwatch.Stop();

                    using (LogContext.PushProperty("PaymentId", payment.PaymentId))
                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    using (LogContext.PushProperty("AuditEvent", payment.Status == PaymentStatus.Completed ? "PaymentCompleted" : "PaymentFailed"))
                    {
                        if (payment.Status == PaymentStatus.Completed)
                        {
                            _logger.LogInformation("AUDIT: Payment {PaymentId} completed successfully for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}, Transaction: {TransactionId}", 
                                payment.PaymentId, request.OrderId, payment.Amount, payment.Method, payment.TransactionId);
                        }
                        else
                        {
                            _logger.LogWarning("AUDIT: Payment {PaymentId} failed for order {OrderId} - Amount: {Amount:C}, Method: {PaymentMethod}, Reason: {FailureReason}", 
                                payment.PaymentId, request.OrderId, payment.Amount, payment.Method, payment.FailureReason);
                        }
                    }

                    return payment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing payment for order {OrderId} with amount {Amount:C}", request.OrderId, request.Amount);
                    throw;
                }
            }
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            using (LogContext.PushProperty("Operation", "GetPayment"))
            {
                _logger.LogDebug("Retrieving payment {PaymentId}", paymentId);

                try
                {
                    await Task.Delay(50);
                    var payment = _payments.FirstOrDefault(p => p.PaymentId == paymentId);

                    if (payment == null)
                    {
                        _logger.LogWarning("Payment {PaymentId} not found", paymentId);
                    }

                    return payment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving payment {PaymentId}", paymentId);
                    throw;
                }
            }
        }

        public async Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            using (LogContext.PushProperty("Operation", "GetPaymentsByOrder"))
            using (LogContext.PushProperty("OrderId", orderId))
            {
                _logger.LogDebug("Retrieving payments for order {OrderId}", orderId);

                try
                {
                    await Task.Delay(100);
                    var payments = _payments.Where(p => p.OrderId == orderId).ToList();

                    _logger.LogDebug("Found {PaymentCount} payments for order {OrderId}", payments.Count, orderId);
                    return payments;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving payments for order {OrderId}", orderId);
                    throw;
                }
            }
        }

        public async Task<Payment> ProcessRefundAsync(RefundRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "ProcessRefund"))
            using (LogContext.PushProperty("PaymentId", request.PaymentId))
            using (LogContext.PushProperty("AuditEvent", "RefundAttempt"))
            {
                _logger.LogInformation("Processing refund for payment {PaymentId} - Amount: {Amount:C}, Reason: {Reason}", 
                    request.PaymentId, request.Amount, request.Reason);

                try
                {
                    var originalPayment = _payments.FirstOrDefault(p => p.PaymentId == request.PaymentId);
                    if (originalPayment == null)
                    {
                        _logger.LogWarning("Refund failed - Payment {PaymentId} not found", request.PaymentId);
                        throw new InvalidOperationException("Payment not found");
                    }

                    if (originalPayment.Status != PaymentStatus.Completed)
                    {
                        _logger.LogWarning("Refund failed - Payment {PaymentId} is not in completed status. Current status: {PaymentStatus}", 
                            request.PaymentId, originalPayment.Status);
                        throw new InvalidOperationException("Can only refund completed payments");
                    }

                    if (request.Amount > originalPayment.Amount)
                    {
                        _logger.LogWarning("Refund failed - Refund amount {RefundAmount:C} exceeds original payment amount {OriginalAmount:C} for payment {PaymentId}", 
                            request.Amount, originalPayment.Amount, request.PaymentId);
                        throw new ArgumentException("Refund amount cannot exceed original payment amount");
                    }

                    // Simulate refund processing
                    await Task.Delay(400);

                    var refundPayment = new Payment
                    {
                        PaymentId = _nextPaymentId++,
                        OrderId = originalPayment.OrderId,
                        Amount = -request.Amount, // Negative amount for refund
                        Method = originalPayment.Method,
                        Status = PaymentStatus.Refunded,
                        ProcessedAt = DateTime.UtcNow,
                        TransactionId = $"REF_{Guid.NewGuid().ToString("N")[..10].ToUpper()}"
                    };

                    _payments.Add(refundPayment);
                    stopwatch.Stop();

                    using (LogContext.PushProperty("RefundPaymentId", refundPayment.PaymentId))
                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    using (LogContext.PushProperty("AuditEvent", "RefundCompleted"))
                    {
                        _logger.LogInformation("AUDIT: Refund {RefundPaymentId} processed successfully for original payment {PaymentId} - Amount: {Amount:C}, Reason: {Reason}", 
                            refundPayment.PaymentId, request.PaymentId, request.Amount, request.Reason);
                    }

                    return refundPayment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing refund for payment {PaymentId} with amount {Amount:C}", request.PaymentId, request.Amount);
                    throw;
                }
            }
        }

        private async Task<bool> SimulatePaymentGatewayAsync(Payment payment)
        {
            using (LogContext.PushProperty("Operation", "PaymentGateway"))
            using (LogContext.PushProperty("PaymentMethod", payment.Method))
            {
                _logger.LogDebug("Communicating with payment gateway for payment {PaymentId}", payment.PaymentId);

                try
                {
                    // Simulate network call to payment gateway
                    await Task.Delay(200);

                    // Simulate random failures (10% failure rate)
                    var random = new Random();
                    var success = random.Next(1, 11) != 1;

                    using (LogContext.PushProperty("Duration", 200))
                    {
                        if (success)
                        {
                            _logger.LogDebug("Payment gateway approved payment {PaymentId}", payment.PaymentId);
                        }
                        else
                        {
                            _logger.LogWarning("Payment gateway declined payment {PaymentId}", payment.PaymentId);
                        }
                    }

                    return success;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Payment gateway communication failed for payment {PaymentId}", payment.PaymentId);
                    return false;
                }
            }
        }
    }
}