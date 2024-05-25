namespace FacturilaAPI.Models.Dto;

public class DashboardStatsDto
{
    public int TotalDocuments { get; set; }
    public int TotalClients { get; set; }
    public int TotalProducts { get; set; }
    public int TotalBankAccounts { get; set; }
    public List<MonthlyTotalDto> MonthlyTotals { get; set; }
}