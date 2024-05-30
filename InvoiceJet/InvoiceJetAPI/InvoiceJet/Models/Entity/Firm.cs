﻿namespace InvoiceJetAPI.Models.Entity
{
    public class Firm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CUI { get; set; }
        public string RegCom { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
        public string City { get; set; }

        public virtual ICollection<UserFirm>? UserFirms { get; set; }
    }
}