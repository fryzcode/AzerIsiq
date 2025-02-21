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
        private readonly IValidator<RegisterDTO> _registerValidator;

        public AuthController(AuthService authService, IValidator<RegisterDTO> registerValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                ValidationResult validationResult = await _registerValidator.ValidateAsync(model);

                if (!validationResult.IsValid)
                {
                    validationResult.AddToModelState(ModelState);
                    return BadRequest(ModelState);
                }
                
                var verificationToken = await _authService.Register(
                    model.UserName,
                    model.Email,
                    model.Password,
                    model.PasswordConfirmation,
                    model.Role
                );

                return Ok(new { Message = "User registered successfully", VerificationToken = verificationToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var response = await _authService.Login(model.Email, model.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
}
