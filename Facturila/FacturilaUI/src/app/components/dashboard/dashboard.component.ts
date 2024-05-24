import { Component } from "@angular/core";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent {
  constructor() {}

  ngOnInit(): void {}

  public barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true,
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
}
