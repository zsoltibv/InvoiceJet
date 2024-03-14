using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Models.Entity;

public class UserFirm
{
    public Guid UserId { get; set; }
    public int FirmId { get; set; }

    public bool IsClient { get; set; } = true;

    public virtual User User { get; set; } = null!;
    public virtual Firm Firm { get; set; } = null!;
}