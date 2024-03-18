using FacturilaAPI.Models.Enums;

namespace FacturilaAPI.Models.Entity
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string Iban { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsActive { get; set; } = false;
        
        public int UserFirmId {  get; set; }
        public virtual UserFirm? UserFirm { get; set; }
    }
}
