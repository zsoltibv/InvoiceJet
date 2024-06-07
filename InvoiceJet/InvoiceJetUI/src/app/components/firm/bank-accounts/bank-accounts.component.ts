import { ToastrModule, ToastrService } from "ngx-toastr";
import { LiveAnnouncer } from "@angular/cdk/a11y";
import { ChangeDetectorRef, Component, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IBankAccount } from "src/app/models/IBankAccount";
import { AuthService } from "src/app/services/auth.service";
import { BankAccountService } from "src/app/services/bank-account.service";
import { AddOrEditBankAccountDialogComponent } from "./add-or-edit-bank-account-dialog/add-or-edit-bank-account-dialog.component";
import { ICurrency } from "src/app/models/ICurrency";
import { Currency } from "src/app/enums/Currency";
import { SelectionModel } from "@angular/cdk/collections";
import { MatSnackBar } from "@angular/material/snack-bar";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: "app-bank-accounts",
  templateUrl: "./bank-accounts.component.html",
  styleUrls: ["./bank-accounts.component.scss"],
})
export class BankAccountsComponent {
  displayedColumns: string[] = [
    "select",
    "bankName",
    "iban",
    "currency",
    "isActive",
  ];
  dataSource = new MatTableDataSource<IBankAccount>();
  selection = new SelectionModel<IBankAccount>(true, []);
  bankAccounts!: IBankAccount[];
  currencies: ICurrency[] = [
    {
      value: Currency.Ron,
      name: "RON",
    },
    {
      value: Currency.Eur,
      name: "EUR",
    },
  ];

  constructor(
    private _liveAnnouncer: LiveAnnouncer,
    public dialog: MatDialog,
    private bankAccountService: BankAccountService,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getUserBankAccounts();
  }

  getUserBankAccounts(): void {
    this.bankAccountService.getUserFirmBankAccounts().subscribe((accounts) => {
      this.bankAccounts = this.mapCurrencyNames(accounts);
      this.dataSource.data = this.bankAccounts;
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

  openNewBankAccountDialog() {
    const dialogRef = this.dialog.open(AddOrEditBankAccountDialogComponent, {});
    dialogRef.afterClosed().subscribe((changed) => {
      if (changed) {
        this.getUserBankAccounts();
      }
      this.selection.clear();
    });
  }

  openEditBankAccountDialog(bankAccount: IBankAccount) {
    this.selection.clear();
    const dialogRef = this.dialog.open(AddOrEditBankAccountDialogComponent, {
      disableClose: true,
      panelClass: "custom-dialog-panel",
      data: bankAccount,
    });

    dialogRef.afterClosed().subscribe((changed) => {
      if (changed) {
        this.getUserBankAccounts();
      }
      this.selection.clear();
    });
  }

  mapCurrencyNames(accounts: IBankAccount[]): IBankAccount[] {
    return accounts.map((account) => {
      const currency = this.currencies.find(
        (c) => c.value === account.currency
      );
      return {
        ...account,
        currencyName: currency ? currency.name : "Unknown",
      };
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
  }

  deleteSelected() {
    const selectedIds = this.selection.selected.map((s) => s.id);
    if (selectedIds.length !== 0) {
      this.bankAccountService.deleteBankAccounts(selectedIds).subscribe(() => {
        this.getUserBankAccounts();
        this.selection.clear();
        this.toastr.success("Bank accounts deleted successfully.", "Success");
      });
    }
  }
}
