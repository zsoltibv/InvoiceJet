import { SelectionModel } from "@angular/cdk/collections";
import { Component } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { Router } from "@angular/router";
import { IDocumentTableRecord } from "src/app/models/IDocumentTableRecord";
import { DocumentService } from "src/app/services/document.service";

@Component({
  selector: "app-invoice-stornos",
  templateUrl: "./invoice-stornos.component.html",
  styleUrls: ["./invoice-stornos.component.scss"],
})
export class InvoiceStornosComponent {
  displayedColumns: string[] = [
    "select",
    "documentNumber",
    "clientName",
    "issueDate",
    "dueDate",
    "totalValue",
    "documentStatus",
  ];
  dataSource = new MatTableDataSource<IDocumentTableRecord>([]);
  selection = new SelectionModel<IDocumentTableRecord>(true, []);
  invoices: IDocumentTableRecord[] = [];

  constructor(
    private router: Router,
    private documentService: DocumentService
  ) {}

  ngOnInit() {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.documentService.getDocuments(3).subscribe((invoices) => {
      this.dataSource.data = invoices;
      this.invoices = invoices;
      console.log(this.invoices);
    });
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

  previewStornoInvoice(row: IDocumentTableRecord): void {}

  deleteSelected(): void {
    console.log(this.selection.selected);

    const documentIds: number[] = this.selection.selected.map((doc) => doc.id);
    console.log(documentIds);
    this.documentService.deleteDocuments(documentIds).subscribe({
      next: () => {
        this.loadInvoices();
      },
      error: (err) => {
        console.error("Error deleting documents", err);
      },
    });
  }
}
