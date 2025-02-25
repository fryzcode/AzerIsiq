using AzerIsiq.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AzerIsiq.Dtos;
using AzerIsiq.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AzerIsiq.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IValidator<RegisterDto> _registerValidator;

        public AuthController(AuthService authService, IValidator<RegisterDto> registerValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                ValidationResult validationResult = await _registerValidator.ValidateAsync(model);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState);
                    return BadRequest(ModelState);
                }

                var token = await _authService.RegisterAsync(model);

                return Ok(new { Message = "User registered successfully", Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var response = await _authService.LoginAsync(model);
                return Ok(new { Message = "User login successfully", response });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            bool result = await _authService.ForgotPasswordAsync(dto);
            return result ? Ok(new { Message = "Reset email sent"}) : NotFound(new {Message = "User not found"});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            bool result = await _authService.ResetPasswordAsync(dto);
            return result ? Ok(new { Message = "Password has been reset"}) : BadRequest(new { Message = "Invalid or expired token"});
        }
    }
}
