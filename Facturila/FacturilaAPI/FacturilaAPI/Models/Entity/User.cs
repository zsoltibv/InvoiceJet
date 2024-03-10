namespace FacturilaAPI.Models.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public int? ActiveFirmId { get; set; }
        public Firm ActiveFirm { get; set; }

        public ICollection<UserFirm> UserFirms { get; set; }
    }
}
