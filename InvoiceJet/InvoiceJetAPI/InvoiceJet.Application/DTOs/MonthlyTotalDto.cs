﻿namespace InvoiceJetAPI.Models.Dto;

public class MonthlyTotalDto
{
    public int Month { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal IncomeAmount { get; set; }
}