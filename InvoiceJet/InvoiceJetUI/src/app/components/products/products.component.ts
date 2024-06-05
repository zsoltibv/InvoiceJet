import { AuthService } from "./../../services/auth.service";
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IProduct } from "src/app/models/IProduct";
import { ProductService } from "src/app/services/product.service";
import { AddEditClientDialogComponent } from "../firm/add-edit-client-dialog/add-edit-client-dialog.component";
import { AddOrEditProductDialogComponent } from "./add-or-edit-product-dialog/add-or-edit-product-dialog.component";
import { LiveAnnouncer } from "@angular/cdk/a11y";
import { SelectionModel } from "@angular/cdk/collections";

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
    private authService: AuthService,
    private _liveAnnouncer: LiveAnnouncer
  ) {}

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService
      .getProductsForUserId(this.authService.userId)
      .subscribe((products) => {
        this.products = products;
        this.dataSource.data = this.products;
        console.log(products);
      });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  openNewProductDialog(): void {
    // Logic to open dialog for adding a new product
    const dialogRef = this.dialog.open(AddOrEditProductDialogComponent, {});

    dialogRef.afterClosed().subscribe((result) => {
      this.getProducts(); // Refresh list after adding
    });
  }

  openEditProductDialog(product: IProduct): void {
    console.log(product);
    // Logic to open dialog for editing a product
    const dialogRef = this.dialog.open(AddOrEditProductDialogComponent, {
      data: product,
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.getProducts(); // Refresh list after editing
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
