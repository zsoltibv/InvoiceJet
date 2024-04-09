import { AuthService } from "src/app/services/auth.service";
import { FirmService } from "src/app/services/firm.service";
import { LiveAnnouncer } from "@angular/cdk/a11y";
import { Component, ViewChild } from "@angular/core";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { AddEditClientDialogComponent } from "../add-edit-client-dialog/add-edit-client-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { IFirm } from "src/app/models/IFirm";
import { SelectionModel } from "@angular/cdk/collections";

@Component({
  selector: "app-clients",
  templateUrl: "./clients.component.html",
  styleUrls: ["./clients.component.scss"],
})
export class ClientsComponent {
  displayedColumns: string[] = [
    "select",
    "name",
    "cui",
    "regCom",
    "address",
    "county",
    "city",
  ];
  dataSource = new MatTableDataSource<IFirm>();
  selection = new SelectionModel<IFirm>(true, []);
  firms: IFirm[] = [];

  constructor(
    private _liveAnnouncer: LiveAnnouncer,
    public dialog: MatDialog,
    private firmService: FirmService,
    private authService: AuthService
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getUserFirms();
  }

  getUserFirms(): void {
    this.firmService
      .getUserClientFirmsById(this.authService.userId)
      .subscribe((firms) => {
        this.firms = firms;
        this.dataSource.data = this.firms;
        console.log(firms);
      });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  announceSortChange(sortState: any) {
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce("Sorting cleared");
    }
  }

  openNewClientDialog() {
    const dialogRef = this.dialog.open(AddEditClientDialogComponent, {});

    dialogRef.afterClosed().subscribe(() => {
      this.getUserFirms();
    });
  }

  openEditClientDialog(firm: IFirm) {
    const dialogRef = this.dialog.open(AddEditClientDialogComponent, {
      data: firm,
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.getUserFirms();
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
  }

  deleteSelected() {
    const selectedIds = this.selection.selected.map((s) => s.id); // Get IDs of selected items
    console.log(selectedIds); // Implement deletion logic here
    // After deletion, update the dataSource and clear selection
    // this.dataSource.data = newData;
    this.selection.clear();
  }
}
