using AccountingAppBackend.Models;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Services.INF;
using Microsoft.AspNetCore.Mvc;

namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = "帳號和密碼不能為空。"
                });
            }

            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = result.Token,
                Message = result.Message
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = "帳號和密碼不能為空。"
                });
            }

            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
            {
                if (result.Message == "帳號已存在。")
                {
                    return Conflict(new ApiResponse<string>
                    {
                        Success = false,
                        Data = null,
                        Message = result.Message
                    });
                }
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = null,
                Message = result.Message
            });
        }
    }
}