import { LiveAnnouncer } from "@angular/cdk/a11y";
import { Component, ViewChild } from '@angular/core';
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IBankAccount } from "src/app/models/IBankAccount";
import { AuthService } from "src/app/services/auth.service";
import { BankAccountService } from "src/app/services/bank-account.service";
import { AddOrEditBankAccountDialogComponent } from "./add-or-edit-bank-account-dialog/add-or-edit-bank-account-dialog.component";

@Component({
  selector: 'app-bank-accounts',
  templateUrl: './bank-accounts.component.html',
  styleUrls: ['./bank-accounts.component.scss']
})
export class BankAccountsComponent {
  displayedColumns: string[] = ['bankName', 'iban', 'currency', 'isActive'];
  dataSource = new MatTableDataSource<IBankAccount>();
  bankAccounts!: IBankAccount[];

  constructor(
    private _liveAnnouncer: LiveAnnouncer,
    public dialog: MatDialog,
    private bankAccountService: BankAccountService,
    private authService: AuthService
  ) { }

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.bankAccountService.getUserFirmBankAccounts(this.authService.userId).subscribe((accounts) => {
      console.log(accounts);
      this.bankAccounts = accounts;
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
      console.log('The dialog was closed');
    });
  }
}
