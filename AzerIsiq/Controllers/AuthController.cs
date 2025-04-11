using AzerIsiq.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AzerIsiq.Dtos;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AzerIsiq.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;

    public AuthController(IAuthService authService, IValidator<RegisterDto> registerValidator, IValidator<ResetPasswordDto> resetPasswordValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _resetPasswordValidator = resetPasswordValidator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            ValidationResult validationResult = await _registerValidator.ValidateAsync(dto);
            
            string ipAddress = GetIpAddress();

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var token = await _authService.RegisterAsync(dto, ipAddress);

            return Ok(new { Message = "User registered successfully", Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            string ipAddress = GetIpAddress();
            var response = await _authService.LoginAsync(dto, ipAddress);
            return Ok(new { Message = "User login successfully", response });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }
    
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpDto dto)
    {
        string ipAddress = GetIpAddress();
        var authResponse = await _authService.VerifyOtpAsync(dto.Email, dto.OtpCode, ipAddress);
        return Ok(authResponse);
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
        ValidationResult validationResult = await _resetPasswordValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        bool result = await _authService.ResetPasswordAsync(dto);
        return result ? Ok(new { Message = "Password has been reset"}) : BadRequest(new { Message = "Invalid or expired token"});
    }
    
    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();
        }
        
        string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        if (ip == "::1")
            ip = "127.0.0.1";

        return ip;
    }

}
