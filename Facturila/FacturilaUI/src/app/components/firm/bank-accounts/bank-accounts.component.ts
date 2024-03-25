import { LiveAnnouncer } from "@angular/cdk/a11y";
import { Component, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-bank-accounts',
  templateUrl: './bank-accounts.component.html',
  styleUrls: ['./bank-accounts.component.scss']
})
export class BankAccountsComponent {
  displayedColumns: string[] = ['select', 'bankName', 'iban', 'currency', 'isActive'];
  dataSource = new MatTableDataSource<IBankAccount>();
  selection = new SelectionModel<IBankAccount>(true, []);
  bankAccounts!: IBankAccount[];
  currencies: ICurrency[] = [
    {
      value: Currency.Ron,
      name: 'RON'
    },
    {
      value: Currency.Eur,
      name: 'EUR'
    }
  ]

  constructor(
    private _liveAnnouncer: LiveAnnouncer,
    public dialog: MatDialog,
    private bankAccountService: BankAccountService,
    private authService: AuthService
  ) { }

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getUserBankAccounts();
  }

  getUserBankAccounts(): void {
    this.bankAccountService.getUserFirmBankAccounts(this.authService.userId).subscribe((accounts) => {
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
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }

  openNewBankAccountDialog() {
    const dialogRef = this.dialog.open(AddOrEditBankAccountDialogComponent, {
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getUserBankAccounts();
    });
  }

  openEditBankAccountDialog(bankAccount: IBankAccount) {
    const dialogRef = this.dialog.open(AddOrEditBankAccountDialogComponent, {
      data: bankAccount
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getUserBankAccounts();
    });
  }

  mapCurrencyNames(accounts: IBankAccount[]): IBankAccount[] {
    return accounts.map(account => {
      const currency = this.currencies.find(c => c.value === account.currency);
      return {
        ...account,
        currencyName: currency ? currency.name : 'Unknown'
      };
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  deleteSelected() {
    const selectedIds = this.selection.selected.map(s => s.id); // Get IDs of selected items
    console.log(selectedIds); // Implement deletion logic here
    // After deletion, update the dataSource and clear selection
    // this.dataSource.data = newData;
    this.selection.clear();
  }
}
