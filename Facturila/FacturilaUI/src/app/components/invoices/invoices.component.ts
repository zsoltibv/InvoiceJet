import { SelectionModel } from "@angular/cdk/collections";
import { Component } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { Router } from "@angular/router";
import { IDocumentTableRecord } from "src/app/models/IDocumentTableRecord";
import { DocumentService } from "src/app/services/document.service";

@Component({
  selector: "app-invoices",
  templateUrl: "./invoices.component.html",
  styleUrls: ["./invoices.component.scss"],
})
export class InvoicesComponent {
  displayedColumns: string[] = [
    "select",
    "documentNumber",
    "clientName",
    "issueDate",
    "dueDate",
    "totalValue",
  ];
  dataSource = new MatTableDataSource<IDocumentTableRecord>([]);
  selection = new SelectionModel<IDocumentTableRecord>(true, []);

  constructor(
    private router: Router,
    private documentService: DocumentService
  ) {}

  ngOnInit() {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.documentService.getDocuments(1).subscribe((invoices) => {
      console.log(invoices);
      this.dataSource.data = invoices;
    });
    // This should call your service to load invoices
    // Placeholder for actual data fetching
    // this.dataSource.data = [
    //   {
    //     id: 1,
    //     documentNumber: "INV001",
    //     clientName: "Client A",
    //     issueDate: new Date(),
    //     dueDate: new Date(),
    //     totalValue: 1000,
    //   },
    //   // Add more invoices as needed
    // ];
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
    } else {
      this.dataSource.data.forEach((row) => this.selection.select(row));
    }
  }

  openNewInvoiceDialog(): void {
    this.router.navigate(["/dashboard/add-or-edit-invoice"]);
  }

  openEditInvoiceDialog(row: IDocumentTableRecord): void {
    // Open a dialog for editing an existing invoice
  }

  deleteSelected(): void {}
}
