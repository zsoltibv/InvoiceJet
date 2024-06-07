import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IProduct } from "src/app/models/IProduct";
import { ProductService } from "src/app/services/product.service";
import { AddOrEditProductDialogComponent } from "./add-or-edit-product-dialog/add-or-edit-product-dialog.component";
import { LiveAnnouncer } from "@angular/cdk/a11y";
import { SelectionModel } from "@angular/cdk/collections";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-products",
  templateUrl: "./products.component.html",
  styleUrls: ["./products.component.scss"],
})
export class ProductsComponent implements OnInit {
  displayedColumns: string[] = [
    "select",
    "name",
    "price",
    "unitOfMeasurement",
    "tvaValue",
    "containsTva",
  ];
  dataSource = new MatTableDataSource<IProduct>();
  products: IProduct[] = [];
  selection = new SelectionModel<IProduct>(true, []);

  constructor(
    public dialog: MatDialog,
    private productService: ProductService,
    private _liveAnnouncer: LiveAnnouncer,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService.getProductsForUserId().subscribe((products) => {
      this.products = products;
      this.dataSource.data = this.products;
    });
  }

  openNewProductDialog(): void {
    const dialogRef = this.dialog.open(AddOrEditProductDialogComponent, {});
    this.selection.clear();

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) this.getProducts();
    });
  }

  openEditProductDialog(product: IProduct): void {
    const dialogRef = this.dialog.open(AddOrEditProductDialogComponent, {
      data: product,
      disableClose: true,
    });
    this.selection.clear();

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) this.getProducts();
    });
  }

  announceSortChange(sortState: any) {
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce("Sorting cleared");
    }
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
    this.productService.deleteProducts(selectedIds).subscribe(() => {
      this.getProducts();
      this.toastr.success("Products deleted successfully!", "Success");
    });
    this.selection.clear();
  }
}
