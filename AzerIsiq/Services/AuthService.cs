using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Register(string userName, string email, string password, string PasswordConfirmation, string roleName)
        {
            if (await _userRepository.ExistsByEmailAsync(email))
                throw new Exception("User with this email already exists");
            
            if (password != PasswordConfirmation)
            {
                throw new Exception("Passwords don't match");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string verificationToken = Guid.NewGuid().ToString();
 
            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = hashedPassword,
                VerificationToken = verificationToken
            };

            await _userRepository.AddAsync(user);

            var role = await _roleRepository.GetByRoleNameAsync(roleName)
                       ?? await _roleRepository.GetByRoleNameAsync("User");

            user.UserRoles = new List<UserRole> { new UserRole { UserId = user.Id, RoleId = role.Id } };

            await _userRepository.SaveChangesAsync();

            return verificationToken;
        }

        // public async Task<string> Login(string email, string password)
        // {
        //     var user = await _userRepository.GetByEmailAsync(email);
        //
        //     if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        //         throw new Exception("Invalid Email or Password");
        //
        //     var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        //
        //     return _jwtService.GenerateJwtToken(user, roles);
        // }
        
        public async Task<AuthResponseDto> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials");
            
            var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

            var token = _jwtService.GenerateJwtToken(user, roles);

            
            return new AuthResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                Token = token
            };
        }

    }
}
