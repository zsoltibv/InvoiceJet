import { Component } from "@angular/core";
import { IDashboardStats } from "src/app/models/IDashboardStats";
import { IMonthlyTotal } from "src/app/models/IMonthlyTotal";
import { DocumentService } from "src/app/services/document.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent {
  dashboardStats: IDashboardStats = {
    totalDocuments: 0,
    totalClients: 0,
    totalProducts: 0,
    totalBankAccounts: 0,
    monthlyTotals: [],
  };

  constructor(private documentService: DocumentService) {}

  ngOnInit(): void {
    this.getDashboardData();
  }

  public barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true,
    maintainAspectRatio: false,
  };
  public barChartLabels = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];
  public barChartType = "bar";
  public barChartLegend = true;

  public barChartData = [
    {
      data: [65, 59, 80, 81, 56, 55, 40, 30, 70, 90, 100, 110],
      label: "Invoice Amount",
    },
    {
      data: [28, 48, 40, 19, 86, 27, 90, 50, 60, 80, 95, 120],
      label: "Income Amount",
    },
  ];

  getDashboardData() {
    this.documentService.getDashboardData().subscribe((data) => {
      this.dashboardStats = data;
      this.updateChartData(data.monthlyTotals);
    });
  }

  updateChartData(monthlyTotals: IMonthlyTotal[]) {
    const invoiceAmounts = new Array(12).fill(0);
    const incomeAmounts = new Array(12).fill(0);

    monthlyTotals.forEach((total) => {
      const index = total.month - 1;
      invoiceAmounts[index] = total.invoiceAmount;
      incomeAmounts[index] = total.incomeAmount;
    });

    this.barChartData = [
      { data: invoiceAmounts, label: "Invoice Amount" },
      { data: incomeAmounts, label: "Income Amount" },
    ];
  }
}
