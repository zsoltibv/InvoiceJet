using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Models.Entity;

public class UserFirm
{
    public int UserFirmId { get; set; } 

    public Guid UserId { get; set; }
    public int FirmId { get; set; }

    public bool IsClient { get; set; } = true;

    public virtual User User { get; set; } = null!;
    public virtual Firm Firm { get; set; } = null!;

    public virtual ICollection<BankAccount>? BankAccounts { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}