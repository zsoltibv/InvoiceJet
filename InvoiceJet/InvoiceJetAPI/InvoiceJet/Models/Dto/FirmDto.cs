﻿using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.Models.Dto
{
    public class FirmDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CUI { get; set; }
        public string? RegCom { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
        public string City { get; set; }
    }
}