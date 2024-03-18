using FacturilaAPI.Models.Entity;
using FacturilaAPI.Models.Enums;

namespace FacturilaAPI.Models.Dto
{
    public class BankAccountDto
    {
        public int? Id { get; set; }
        public string BankName { get; set; }
        public string Iban { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsActive { get; set; }
    }
}
