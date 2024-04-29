import { DocumentService } from "./../../../services/document.service";
import { AuthService } from "./../../../services/auth.service";
import { FirmService } from "./../../../services/firm.service";
import { Component } from "@angular/core";
import {
  Form,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { MatTableDataSource } from "@angular/material/table";
import { Observable, map, merge, startWith } from "rxjs";
import { IFirm } from "src/app/models/IFirm";
import { IDocumentAutofill } from "src/app/models/IDocumentAutofill";
import { IProduct } from "src/app/models/IProduct";

@Component({
  selector: "app-add-or-edit-invoice",
  templateUrl: "./add-or-edit-invoice.component.html",
  styleUrls: ["./add-or-edit-invoice.component.scss"],
})
export class AddOrEditInvoiceComponent {
  invoiceAutofillData: IDocumentAutofill = {} as IDocumentAutofill;
  filteredFirms!: Observable<IFirm[]>;
  filteredProducts!: Observable<any[]>;
  clientFirms: IFirm[] = [];

  dataSource = new MatTableDataSource();
  invoiceForm!: FormGroup;
  displayedColumns: string[] = [
    "name",
    "price",
    "unitOfMeasurement",
    "tvaValue",
    "containsTVA",
    "actions",
  ];

  seriesList = [
    {
      name: "A",
    },
  ];

  constructor(
    private fb: FormBuilder,
    private firmService: FirmService,
    private authService: AuthService,
    private documentService: DocumentService
  ) {}

  ngOnInit(): void {
    this.documentService
      .getDocumentSeriesForUserId(this.authService.userId, 1)
      .subscribe({
        next: (data) => {
          this.invoiceAutofillData = data;
          this.setupClientFilters();
          this.setupProductFilters();
        },
        error: (err) => {
          console.error("Error fetching data", err);
        },
      });

    this.invoiceForm = this.fb.group({
      client: ["", Validators.required],
      issueDate: [new Date(), Validators.required],
      dueDate: "",
      serieSiNumar: ["", Validators.required],
      products: this.fb.array([this.createProductGroup()]),
    });
    this.updateTableData();
  }

  displayFn(firm: IFirm): string {
    return firm && firm.name ? firm.name : "";
  }

  setupClientFilters() {
    this.filteredFirms = this.invoiceForm.get("client")!.valueChanges.pipe(
      startWith(""),
      map((value) => this.filterClients(value))
    );
  }

  setupProductFilters() {
    const products = this.productsFormArray;
    if (products.length === 0) return;

    const nameChangeObservables: Observable<any>[] = products.controls.map(
      (productFormGroup) => {
        return productFormGroup.get("name")!.valueChanges.pipe(
          startWith(""),
          map((value) => this.filterProducts(value))
        );
      }
    );

    this.filteredProducts = merge(...nameChangeObservables);
  }

  filterClients(value: any): IFirm[] {
    let filterValue = "";

    if (typeof value === "string") {
      filterValue = value.toLowerCase();
    } else if (value && typeof value === "object" && value.name) {
      filterValue = value.name.toLowerCase();
    }

    return this.invoiceAutofillData.clients.filter(
      (firm) =>
        firm.name.toLowerCase().includes(filterValue) ||
        firm.cui.toLowerCase().includes(filterValue)
    );
  }

  filterProducts(value: string): any[] {
    console.log("Filtering products", value);
    const filterValue = value.toLowerCase();
    return this.invoiceAutofillData.products.filter((product) =>
      product.name.toLowerCase().includes(filterValue)
    );
  }

  updateTableData() {
    this.dataSource.data = this.getControls();
  }

  createProductGroup(): FormGroup {
    return this.fb.group({
      name: ["", Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      unitOfMeasurement: ["buc", Validators.required],
      tvaValue: [19, [Validators.required, Validators.min(0)]],
      containsTVA: [false],
    });
  }

  get productsFormArray(): FormArray {
    return this.invoiceForm.get("products") as FormArray;
  }

  getControls() {
    return (this.invoiceForm.get("products") as FormArray).controls;
  }

  addProduct(): void {
    console.log(this.productsFormArray.value);
    this.productsFormArray.push(this.createProductGroup());
    this.updateTableData();
    this.setupProductFilters();
  }

  deleteProduct(index: number): void {
    this.productsFormArray.removeAt(index);
    this.updateTableData();
  }

  onSubmit(): void {
    console.log("Form submitted", this.invoiceForm.value);
    if (this.invoiceForm.valid) {
      console.log("Form submitted", this.invoiceForm.value);
    } else {
      console.log("Form is not valid");
    }
  }
}
