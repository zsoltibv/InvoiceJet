import { LiveAnnouncer } from "@angular/cdk/a11y";
import { SelectionModel } from "@angular/cdk/collections";
import { Component, ViewChild } from '@angular/core';
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IDocumentSeries } from "src/app/models/IDocumentSeries";
import { AuthService } from "src/app/services/auth.service";
import { DocumentSeriesService } from "src/app/services/document-series.service";

@Component({
  selector: 'app-document-series',
  templateUrl: './document-series.component.html',
  styleUrls: ['./document-series.component.scss']
})
export class DocumentSeriesComponent {
  displayedColumns: string[] = ['select', 'seriesName', 'firstNumber', 'currentNumber', 'isDefault'];
  dataSource = new MatTableDataSource<IDocumentSeries>();
  selection = new SelectionModel<IDocumentSeries>(true, []);
  documentSeriesList!: IDocumentSeries[];

  constructor(
    public dialog: MatDialog,
    private documentSeriesService: DocumentSeriesService,
    private authService: AuthService,
    private _liveAnnouncer: LiveAnnouncer,
  ) { }

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getDocumentSeries();
  }

  getDocumentSeries(): void {
    this.documentSeriesService.getDocumentSeriesForUserId(this.authService.userId).subscribe((series) => {
      this.documentSeriesList = series;
      this.dataSource.data = this.documentSeriesList;
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  openNewDocumentSeriesDialog() {
    throw new Error('Method not implemented.');
  }
  openEditDocumentSeriesDialog(row: IDocumentSeries) {
    throw new Error('Method not implemented.');
  }

  announceSortChange(sortState: any) {
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
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
