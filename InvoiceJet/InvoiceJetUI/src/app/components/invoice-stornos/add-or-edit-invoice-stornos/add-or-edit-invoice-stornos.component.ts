import { ChangeDetectorRef, Component } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { MatTableDataSource } from "@angular/material/table";
import { ActivatedRoute, Router } from "@angular/router";
import { Observable, map, merge, startWith } from "rxjs";
import { IDocumentAutofill } from "src/app/models/IDocumentAutofill";
import { IDocumentRequest } from "src/app/models/IDocumentRequest";
import { IDocumentStatus } from "src/app/models/IDocumentStatus";
import { IFirm } from "src/app/models/IFirm";
import { IProduct } from "src/app/models/IProduct";
import { AuthService } from "src/app/services/auth.service";
import { DocumentService } from "src/app/services/document.service";
import { FirmService } from "src/app/services/firm.service";
import { PdfViewerComponent } from "../../pdf-viewer/pdf-viewer.component";

@Component({
  selector: "app-add-or-edit-invoice-stornos",
  templateUrl: "./add-or-edit-invoice-stornos.component.html",
  styleUrls: ["./add-or-edit-invoice-stornos.component.scss"],
})
export class AddOrEditInvoiceStornosComponent {
  invoiceAutofillData: IDocumentAutofill = {} as IDocumentAutofill;
  filteredFirms!: Observable<IFirm[]>;
  filteredProducts!: Observable<IProduct[]>;
  clientFirms: IFirm[] = [];
  currentDocument!: IDocumentRequest;
  currentStatus!: IDocumentStatus;

  dataSource = new MatTableDataSource();
  invoiceForm: FormGroup = {} as FormGroup;
  displayedColumns: string[] = [
    "name",
    "unitPrice",
    "quantity",
    "unitOfMeasurement",
    "tvaValue",
    "containsTva",
    "totalPrice",
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
    private documentService: DocumentService,
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const invoiceId = +params["id"]; // '+' converts string to number
      if (invoiceId) {
        this.documentService.getDocumentById(invoiceId).subscribe({
          next: (data) => {
            this.currentDocument = data;
            console.log("Current document", this.currentDocument);
            this.invoiceForm.patchValue(data);
            this.invoiceForm.setControl(
              "products",
              this.fb.array(
                data.products.map((product) => this.fb.group(product))
              )
            );
            this.updateTableData();
          },
          error: (err) => {
            console.error("Error loading invoice", err);
          },
        });
      }
    });

    this.documentService.getDocumentAutofillInfo(3).subscribe({
      next: (data) => {
        this.invoiceAutofillData = data;
        if (this.currentDocument) {
          this.invoiceForm.patchValue({
            documentStatus: this.invoiceAutofillData.documentStatuses.find(
              (status) => status.id === this.currentDocument.documentStatus.id
            ),
          });
          this.cdr.detectChanges();
        }
        this.setupClientFilters();
        this.setupProductFilters();
      },
      error: (err) => {
        console.error("Error fetching data", err);
      },
    });

    this.invoiceForm = this.fb.group({
      id: 0,
      client: [null, Validators.required],
      issueDate: [new Date(), Validators.required],
      dueDate: null,
      documentSeries: [null],
      documentStatus: [null],
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
      id: 0,
      name: [null, Validators.required],
      unitPrice: [null, [Validators.required, Validators.min(0)]],
      totalPrice: [null, [Validators.required, Validators.min(0)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitOfMeasurement: ["buc", Validators.required],
      tvaValue: [19, [Validators.required, Validators.min(0)]],
      containsTva: [false],
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

  onProductSelected(event: any, index: number): void {
    const selectedProduct = this.invoiceAutofillData.products.find(
      (product) => product.name === event.option.value
    );
    if (selectedProduct) {
      const productGroup = this.productsFormArray.at(index) as FormGroup;
      productGroup.patchValue({
        id: selectedProduct.id,
        unitPrice: selectedProduct.price,
        unitOfMeasurement: selectedProduct.unitOfMeasurement,
        tvaValue: selectedProduct.tvaValue,
        containsTva: selectedProduct.containsTva,
      });
      this.calculateTotalPrice(index);
    }
  }

  calculateTotalPrice(index: number): void {
    const productGroup = this.productsFormArray.at(index) as FormGroup;
    const unitPrice = parseFloat(productGroup.get("unitPrice")!.value);
    const quantity = parseFloat(productGroup.get("quantity")!.value);
    const tvaValue = parseFloat(productGroup.get("tvaValue")!.value);

    const totalPrice = unitPrice * quantity;
    const tva = totalPrice * (tvaValue / 100);
    const finalPrice = totalPrice + tva;

    const finalPriceRounded = parseFloat(finalPrice.toFixed(2));

    productGroup.patchValue({
      totalPrice: finalPriceRounded,
    });
  }

  calculatePriceWithoutTVA(index: number, isChecked: boolean): void {
    console.log("Checkbox checked", isChecked);

    const productGroup = this.productsFormArray.at(index) as FormGroup;
    const unitPrice = parseFloat(productGroup.get("unitPrice")!.value);
    const quantity = parseFloat(productGroup.get("quantity")!.value);
    const tvaValue = parseFloat(productGroup.get("tvaValue")!.value);

    if (isChecked) {
      const newTotalPrice = unitPrice * quantity;
      const newUnitPrice = newTotalPrice / quantity / (1 + tvaValue / 100);

      productGroup.patchValue({
        unitPrice: parseFloat(newUnitPrice.toFixed(2)),
        totalPrice: parseFloat(newTotalPrice.toFixed(2)),
      });
    } else {
      const unitPrice = parseFloat(productGroup.get("unitPrice").value);
      const quantity = parseFloat(productGroup.get("quantity").value);
      const tvaValue = parseFloat(productGroup.get("tvaValue").value);

      const originalUnitPrice = unitPrice * (1 + tvaValue / 100);
      const originalTotalPrice =
        originalUnitPrice * quantity +
        (originalUnitPrice * quantity * tvaValue) / 100;

      productGroup.patchValue({
        unitPrice: parseFloat(originalUnitPrice.toFixed(2)),
        totalPrice: parseFloat(originalTotalPrice.toFixed(2)),
      });
    }
  }

  onSubmit(): void {
    if (this.invoiceForm.invalid) return;

    const documentData: IDocumentRequest = this.invoiceForm.value;
    documentData.documentType = { id: 3, name: "" };

    if (!this.isEditMode) {
      console.log("Updating invoice");
      this.updateDocument(documentData);
    } else {
      console.log("Adding invoice");
      this.addDocument(documentData);
    }
  }

  updateDocument(documentData: IDocumentRequest) {
    this.documentService.updateDocument(documentData).subscribe({
      next: () => {
        console.log("Invoice updated successfully");
        this.router.navigateByUrl("/dashboard/invoice-stornos");
      },
      error: (err) => {
        console.error("Error updating invoice", err);
      },
    });
  }

  addDocument(documentData: IDocumentRequest) {
    this.documentService.addDocument(documentData).subscribe({
      next: () => {
        console.log("Invoice added successfully");
        this.router.navigateByUrl("/dashboard/invoice-stornos");
      },
      error: (err) => {
        console.error("Error adding invoice", err);
      },
    });
  }

  generateInvoicePdf() {
    const documentData: IDocumentRequest = this.invoiceForm.value;

    this.documentService.generateDocumentPdf(documentData).subscribe({
      next: () => {
        console.log("Invoice pdf generated successfully");
      },
      error: (err) => {
        console.error("Error generating invoice pdf", err);
      },
    });
  }

  getInvoicePdfStream() {
    // if (this.invoiceForm.invalid) return;

    const documentData: IDocumentRequest = this.invoiceForm.value;
    documentData.documentType = { id: 3, name: "" };
    documentData.documentNumber = this.currentDocument.documentNumber;

    this.documentService.getGeneratedDocumentPdf(documentData).subscribe({
      next: (data) => {
        const blob = new Blob([data], { type: "application/pdf" });
        const url = window.URL.createObjectURL(blob);

        this.dialog.open(PdfViewerComponent, {
          data: { pdfUrl: url },
          width: "100vw",
          height: "90vh",
          disableClose: true,
        });
      },
      error: (err) => {
        console.error("Error getting invoice pdf stream", err);
      },
    });
  }

  goBack(): void {
    this.router.navigateByUrl("/dashboard/invoice-stornos");
  }

  get isEditMode(): boolean {
    return this.currentDocument == undefined;
  }

  get documentNumber(): string {
    return this.currentDocument?.documentNumber || "";
  }

  get documentStatus(): string {
    return this.currentDocument?.documentStatus?.status || "";
  }

  get contentIsLoaded(): boolean {
    if (!this.invoiceAutofillData.clients) return false;
    if (this.invoiceAutofillData.clients.length === 0) return false;
    return true;
  }
}
