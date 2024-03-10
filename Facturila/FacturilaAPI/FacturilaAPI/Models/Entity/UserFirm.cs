using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Models.Entity;

public class UserFirm
{
    public Guid UserId { get; set; }
    public int FirmId { get; set; }

    public bool IsClient { get; set; } = true;

    public User User { get; set; }
    public Firm Firm { get; set; }
}