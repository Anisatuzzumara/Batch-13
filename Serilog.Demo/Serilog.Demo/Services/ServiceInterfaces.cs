using Serilog.Demo.Models;

namespace Serilog.Demo.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(int userId, CreateUserRequest request);
        Task<bool> DeleteUserAsync(int userId);
    }

    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }

    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request);
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task<Payment> ProcessRefundAsync(RefundRequest request);
    }
}