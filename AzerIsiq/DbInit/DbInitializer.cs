using AzerIsiq.Data;
using AzerIsiq.Models;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Extensions.DbInit;

    public class DbInitializer : IDbInitializer
    {

        private readonly AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

            // Проверяем, существуют ли роли
            if (!_context.Roles.Any())
            {
                var roles = new List<Role>
        {
            new Role { RoleName = "Admin" },
            new Role { RoleName = "User" },
            new Role { RoleName = "Manager" },
            new Role { RoleName = "Operator" }
        };
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }

            if (!_context.Users.Any(u => u.Email == "feryazhajimuradov18@gmail.com"))
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "feryazhajimuradov18@gmail.com",
                    PhoneNumber = "994559159999",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345Test@@@@"),
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    IpAddress = "127.0.0.1"
                };
                _context.Users.Add(admin);
                _context.SaveChanges();

                var adminRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                if (adminRole != null)
                {
                    _context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
                    _context.SaveChanges();
                }
            }
        }

    }