import { AuthService } from 'src/app/services/auth.service';
import { FirmService } from 'src/app/services/firm.service';
import { LiveAnnouncer } from "@angular/cdk/a11y";
import { Component, ViewChild } from '@angular/core';
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { AddEditClientDialogComponent } from "../add-edit-client-dialog/add-edit-client-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { IFirm } from "src/app/models/IFirm";

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent {
  displayedColumns: string[] = ['name', 'cui', 'regCom', 'address', 'county', 'city'];
  dataSource = new MatTableDataSource<IFirm>();
  firms!: IFirm[];

  constructor(private _liveAnnouncer: LiveAnnouncer, public dialog: MatDialog, private firmService: FirmService, private authService: AuthService) { }

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.firmService.getUserClientFirmsById(this.authService.userId).subscribe((firms) => {
      this.firms = firms;
      this.dataSource.data = this.firms;
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

  openNewClientDialog() {
    const dialogRef = this.dialog.open(AddEditClientDialogComponent, {
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }
}
