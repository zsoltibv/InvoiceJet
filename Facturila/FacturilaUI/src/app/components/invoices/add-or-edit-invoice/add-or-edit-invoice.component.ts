import { Component } from "@angular/core";
import { FormArray, FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-add-or-edit-invoice",
  templateUrl: "./add-or-edit-invoice.component.html",
  styleUrls: ["./add-or-edit-invoice.component.scss"],
})
export class AddOrEditInvoiceComponent {
  invoiceForm: FormGroup;
  displayedColumns: string[] = [
    "name",
    "price",
    "unitOfMeasurement",
    "tvaValue",
    "containsTVA",
  ];

  seriesList = [
    {
      name: "A",
    },
  ];

  constructor() {
    this.invoiceForm = new FormGroup({
      cuiValue: new FormControl("", Validators.required),
      issueDate: new FormControl(new Date(), Validators.required),
      dueDate: new FormControl("", Validators.required),
      serieSiNumar: new FormControl("", Validators.required),
      products: new FormArray([]),
    });
  }

  ngOnInit(): void {
    // Initialize with any necessary data
  }

  get productsFormArray(): FormArray {
    return this.invoiceForm.get("products") as FormArray;
  }

  addProduct(): void {
    const productGroup = new FormGroup({
      name: new FormControl("", Validators.required),
      price: new FormControl(0, [Validators.required, Validators.min(0)]),
      unitOfMeasurement: new FormControl("", Validators.required),
      tvaValue: new FormControl(0, [Validators.required, Validators.min(0)]),
      containsTVA: new FormControl(false),
    });

    (this.invoiceForm.get("products") as FormArray).push(productGroup);
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
