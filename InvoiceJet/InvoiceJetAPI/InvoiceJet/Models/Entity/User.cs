namespace InvoiceJetAPI.Models.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public int? ActiveUserFirmId { get; set; }
        public virtual UserFirm ActiveUserFirm { get; set; } = null!;

        public virtual ICollection<UserFirm>? UserFirms { get; set; }
    }
}
