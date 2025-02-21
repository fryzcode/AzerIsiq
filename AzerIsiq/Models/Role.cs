namespace AzerIsiq.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public List<UserRole> UserRoles { get; set; } = new();
    }
}
