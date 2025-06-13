using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IUserService _userService;
        private static readonly List<Order> _orders = new();
        private static int _nextOrderId = 1;

        public OrderService(ILogger<OrderService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "CreateOrder"))
            using (LogContext.PushProperty("UserId", request.UserId))
            {
                _logger.LogInformation("Processing order creation for user {UserId} with {ItemCount} items", 
                    request.UserId, request.Items.Count);

                try
                {
                    // Validate user exists
                    var user = await _userService.GetUserByIdAsync(request.UserId);
                    if (user == null)
                    {
                        _logger.LogWarning("Order creation failed - User {UserId} not found", request.UserId);
                        throw new InvalidOperationException("User not found");
                    }

                    // Validate items
                    if (!request.Items.Any())
                    {
                        _logger.LogWarning("Order creation failed - No items provided for user {UserId}", request.UserId);
                        throw new ArgumentException("Order must contain at least one item");
                    }

                    var totalAmount = request.Items.Sum(item => item.TotalPrice);

                    var order = new Order
                    {
                        OrderId = _nextOrderId++,
                        UserId = request.UserId,
                        Items = request.Items,
                        TotalAmount = totalAmount,
                        ShippingAddress = request.ShippingAddress,
                        Status = OrderStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };

                    // Simulate database save
                    await Task.Delay(300);
                    _orders.Add(order);

                    stopwatch.Stop();

                    using (LogContext.PushProperty("OrderId", order.OrderId))
                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    {
                        _logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total amount {TotalAmount:C}", 
                            order.OrderId, request.UserId, order.TotalAmount);

                        // Log individual items for audit trail
                        foreach (var item in order.Items)
                        {
                            _logger.LogDebug("Order {OrderId} contains: {ProductName} x{Quantity} at {UnitPrice:C} each", 
                                order.OrderId, item.ProductName, item.Quantity, item.UnitPrice);
                        }
                    }

                    return order;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating order for user {UserId}", request.UserId);
                    throw;
                }
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            using (LogContext.PushProperty("Operation", "GetOrder"))
            {
                _logger.LogDebug("Retrieving order {OrderId}", orderId);

                try
                {
                    await Task.Delay(50);
                    var order = _orders.FirstOrDefault(o => o.OrderId == orderId);

                    if (order == null)
                    {
                        _logger.LogWarning("Order {OrderId} not found", orderId);
                    }

                    return order;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving order {OrderId}", orderId);
                    throw;
                }
            }
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            using (LogContext.PushProperty("Operation", "GetOrdersByUser"))
            using (LogContext.PushProperty("UserId", userId))
            {
                _logger.LogDebug("Retrieving orders for user {UserId}", userId);

                try
                {
                    await Task.Delay(100);
                    var orders = _orders.Where(o => o.UserId == userId).ToList();

                    _logger.LogDebug("Found {OrderCount} orders for user {UserId}", orders.Count, userId);
                    return orders;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);
                    throw;
                }
            }
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "UpdateOrderStatus"))
            using (LogContext.PushProperty("OrderId", orderId))
            {
                _logger.LogInformation("Updating order {OrderId} status to {NewStatus}", orderId, status);

                try
                {
                    var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order == null)
                    {
                        _logger.LogWarning("Status update failed - Order {OrderId} not found", orderId);
                        throw new InvalidOperationException("Order not found");
                    }

                    var oldStatus = order.Status;
                    order.Status = status;

                    if (status == OrderStatus.Delivered)
                    {
                        order.CompletedAt = DateTime.UtcNow;
                    }

                    await Task.Delay(100);

                    _logger.LogInformation("Order {OrderId} status updated from {OldStatus} to {NewStatus}", 
                        orderId, oldStatus, status);

                    return order;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating status for order {OrderId} to {NewStatus}", orderId, status);
                    throw;
                }
            }
        }
    }
}
