import { ChangeDetectorRef, Component } from "@angular/core";
import {
  Form,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";

@Component({
  selector: "app-add-or-edit-invoice",
  templateUrl: "./add-or-edit-invoice.component.html",
  styleUrls: ["./add-or-edit-invoice.component.scss"],
})
export class AddOrEditInvoiceComponent {
  invoiceForm!: FormGroup;
  displayedColumns: string[] = [
    "index",
    "name",
    "price",
    "unitOfMeasurement",
    "tvaValue",
    "containsTVA",
    "delete",
  ];

  seriesList = [
    {
      name: "A",
    },
  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.invoiceForm = this.fb.group({
      cuiValue: ["", Validators.required],
      issueDate: [new Date(), Validators.required],
      dueDate: ["", Validators.required],
      serieSiNumar: ["", Validators.required],
      products: this.fb.array([this.createProductGroup()]),
    });
  }

  createProductGroup(): FormGroup {
    return this.fb.group({
      name: ["", Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      unitOfMeasurement: ["", Validators.required],
      tvaValue: [0, [Validators.required, Validators.min(0)]],
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
  }

  deleteProduct(index: number): void {
    this.productsFormArray.removeAt(index);
    console.log("Product deleted", this.productsFormArray.value);
  }

  onSubmit(): void {
    if (this.invoiceForm.valid) {
      // Handle form submission
      console.log("Form submitted", this.invoiceForm.value);
    } else {
      // Handle form errors
      console.log("Form is not valid");
    }
  }
}
