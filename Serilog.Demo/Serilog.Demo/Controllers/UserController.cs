using Microsoft.AspNetCore.Mvc;
using Serilog.Demo.Models;
using Serilog.Demo.Services;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog.Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<User>>> Login([FromBody] LoginRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Endpoint", "POST /api/users/login"))
            {
                _logger.LogInformation("User login attempt for username {Username}", request.Username);

                try
                {
                    var user = await _userService.AuthenticateAsync(request.Username, request.Password);
                    stopwatch.Stop();

                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    {
                        if (user != null)
                        {
                            _logger.LogInformation("Login successful for username {Username} in {Duration}ms", 
                                request.Username, stopwatch.ElapsedMilliseconds);

                            return Ok(new ApiResponse<User>
                            {
                                Success = true,
                                Data = user,
                                Message = "Login successful",
                                CorrelationId = correlationId
                            });
                        }
                        else
                        {
                            _logger.LogWarning("Login failed for username {Username} in {Duration}ms", 
                                request.Username, stopwatch.ElapsedMilliseconds);

                            return Unauthorized(new ApiResponse<User>
                            {
                                Success = false,
                                Message = "Invalid username or password",
                                CorrelationId = correlationId
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during login attempt for username {Username}", request.Username);

                    return StatusCode(500, new ApiResponse<User>
                    {
                        Success = false,
                        Message = "An error occurred during login",
                        CorrelationId = correlationId
                    });
                }
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(int userId)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Endpoint", "GET /api/users/{userId}"))
            {
                _logger.LogDebug("Retrieving user {UserId}", userId);

                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);

                    if (user == null)
                    {
                        _logger.LogWarning("User {UserId} not found", userId);
                        return NotFound(new ApiResponse<User>
                        {
                            Success = false,
                            Message = "User not found",
                            CorrelationId = correlationId
                        });
                    }

                    _logger.LogDebug("User {UserId} retrieved successfully", userId);
                    return Ok(new ApiResponse<User>
                    {
                        Success = true,
                        Data = user,
                        Message = "User retrieved successfully",
                        CorrelationId = correlationId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving user {UserId}", userId);

                    return StatusCode(500, new ApiResponse<User>
                    {
                        Success = false,
                        Message = "An error occurred while retrieving the user",
                        CorrelationId = correlationId
                    });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Endpoint", "POST /api/users"))
            {
                _logger.LogInformation("Creating user with username {Username}", request.Username);

                try
                {
                    var user = await _userService.CreateUserAsync(request);
                    stopwatch.Stop();

                    using (LogContext.PushProperty("Duration", stopwatch.ElapsedMilliseconds))
                    {
                        _logger.LogInformation("User created successfully - UserId: {UserId}, Username: {Username} in {Duration}ms", 
                            user.UserId, user.Username, stopwatch.ElapsedMilliseconds);
                    }

                    return CreatedAtAction(nameof(GetUser), new { userId = user.UserId }, new ApiResponse<User>
                    {
                        Success = true,
                        Data = user,
                        Message = "User created successfully",
                        CorrelationId = correlationId
                    });
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "User creation failed for username {Username} - {ErrorMessage}", 
                        request.Username, ex.Message);

                    return BadRequest(new ApiResponse<User>
                    {
                        Success = false,
                        Message = ex.Message,
                        CorrelationId = correlationId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user with username {Username}", request.Username);

                    return StatusCode(500, new ApiResponse<User>
                    {
                        Success = false,
                        Message = "An error occurred while creating the user",
                        CorrelationId = correlationId
                    });
                }
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ApiResponse<User>>> UpdateUser(int userId, [FromBody] CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString("N")[..8];

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("Endpoint", "PUT /api/users/{userId}"))
            {
                _logger.LogInformation("Updating user {UserId}", userId);

                try
                {
                    var user = await _userService.UpdateUserAsync(userId, request);

                    _logger.LogInformation("User {UserId} updated successfully", userId);
                    return Ok(new ApiResponse<User>
                    {
                        Success = true,
                        Data = user,
                        Message = "User updated successfully",
                        CorrelationId = correlationId
                    });
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "User update failed for UserId {UserId} - {ErrorMessage}", userId, ex.Message);

                    return BadRequest(new ApiResponse<User>
                    {
                        Success = false,
                        Message = ex.Message,
                        CorrelationId = correlationId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user {UserId}", userId);

                    return StatusCode(500, new ApiResponse<User>
                    {
                        Success = false,
                        Message = "An error occurred while updating the user",
                        CorrelationId = correlationId
                    });
                }
            }
        }
    }
}