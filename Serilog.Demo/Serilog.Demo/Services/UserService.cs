using Serilog.Demo.Models;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private static readonly List<User> _users = new();
        private static int _nextUserId = 1;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "UserAuthentication"))
            {
                _logger.LogInformation("Authentication attempt for username {Username}", username);

                try
                {
                    // Simulate async database call
                    await Task.Delay(100);

                    var user = _users.FirstOrDefault(u => u.Username == username && u.IsActive);

                    if (user == null)
                    {
                        _logger.LogWarning("Authentication failed - User {Username} not found", username);
                        return null;
                    }

                    // In real implementation, verify password hash
                    if (password != "password123")
                    {
                        _logger.LogWarning("Authentication failed - Invalid password for user {Username}", username);
                        return null;
                    }

                    stopwatch.Stop();
                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    {
                        _logger.LogInformation("Authentication successful for user {Username} (UserId: {UserId})", 
                            username, user.UserId);
                    }

                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during authentication for username {Username}", username);
                    throw;
                }
            }
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "CreateUser"))
            {
                _logger.LogInformation("Creating user with username {Username} and email {Email}", 
                    request.Username, request.Email);

                try
                {
                    // Check for existing user
                    if (_users.Any(u => u.Username == request.Username))
                    {
                        _logger.LogWarning("User creation failed - Username {Username} already exists", request.Username);
                        throw new InvalidOperationException("Username already exists");
                    }

                    if (_users.Any(u => u.Email == request.Email))
                    {
                        _logger.LogWarning("User creation failed - Email {Email} already exists", request.Email);
                        throw new InvalidOperationException("Email already exists");
                    }

                    // Simulate async database operation
                    await Task.Delay(200);

                    var user = new User
                    {
                        UserId = _nextUserId++,
                        Username = request.Username,
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _users.Add(user);
                    stopwatch.Stop();

                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    using (LogContext.PushProperty("UserId", user.UserId))
                    {
                        _logger.LogInformation("User created successfully - UserId: {UserId}, Username: {Username}", 
                            user.UserId, user.Username);
                    }

                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user with username {Username}", request.Username);
                    throw;
                }
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using (LogContext.PushProperty("Operation", "GetUser"))
            {
                _logger.LogDebug("Retrieving user with ID {UserId}", userId);

                try
                {
                    await Task.Delay(50);
                    var user = _users.FirstOrDefault(u => u.UserId == userId);

                    if (user == null)
                    {
                        _logger.LogWarning("User with ID {UserId} not found", userId);
                    }

                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
                    throw;
                }
            }
        }

        public async Task<User> UpdateUserAsync(int userId, CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "UpdateUser"))
            {
                _logger.LogInformation("Updating user {UserId} with new data", userId);

                try
                {
                    var user = _users.FirstOrDefault(u => u.UserId == userId);
                    if (user == null)
                    {
                        _logger.LogWarning("Update failed - User {UserId} not found", userId);
                        throw new InvalidOperationException("User not found");
                    }

                    // Check for username/email conflicts with other users
                    if (_users.Any(u => u.UserId != userId && u.Username == request.Username))
                    {
                        _logger.LogWarning("Update failed - Username {Username} already exists for another user", 
                            request.Username);
                        throw new InvalidOperationException("Username already exists");
                    }

                    await Task.Delay(150);

                    user.Username = request.Username;
                    user.Email = request.Email;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;

                    _logger.LogInformation("User {UserId} updated successfully", userId);
                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user {UserId}", userId);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Operation", "DeleteUser"))
            {
                _logger.LogInformation("Attempting to delete user {UserId}", userId);

                try
                {
                    var user = _users.FirstOrDefault(u => u.UserId == userId);
                    if (user == null)
                    {
                        _logger.LogWarning("Delete failed - User {UserId} not found", userId);
                        return false;
                    }

                    await Task.Delay(100);

                    // Soft delete by marking as inactive
                    user.IsActive = false;

                    _logger.LogInformation("User {UserId} deleted successfully (soft delete)", userId);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting user {UserId}", userId);
                    throw;
                }
            }
        }
    }
}
