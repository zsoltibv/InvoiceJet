import { ToastrModule, ToastrService } from "ngx-toastr";
import { AuthService } from "./../../../services/auth.service";
import { ProductService } from "src/app/services/product.service";
import { Component, Inject, Input, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { IProduct } from "src/app/models/IProduct";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: "app-add-or-edit-product-dialog",
  templateUrl: "./add-or-edit-product-dialog.component.html",
  styleUrls: ["./add-or-edit-product-dialog.component.scss"],
})
export class AddOrEditProductDialogComponent implements OnInit {
  productForm!: FormGroup;
  isEditMode: boolean = false;
  errorMessage!: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IProduct,
    private fb: FormBuilder,
    private productService: ProductService,
    private dialogRef: MatDialogRef<AddOrEditProductDialogComponent>,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.productForm = new FormGroup({
      id: new FormControl(null),
      name: new FormControl("", Validators.required),
      price: new FormControl("", Validators.required),
      containsTva: new FormControl(false),
      tvaValue: new FormControl(19, Validators.required),
      unitOfMeasurement: new FormControl(""),
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.isEditMode = true;
      this.productForm.setValue({
        id: this.data?.id! ?? 0,
        name: this.data.name,
        price: this.data.price,
        containsTva: this.data.containsTva,
        tvaValue: this.data.tvaValue,
        unitOfMeasurement: this.data.unitOfMeasurement,
      });
    }
  }

  initForm(): void {}

  onSubmit(): void {
    if (this.productForm.valid) {
      const productData: IProduct = this.productForm.value;
      productData.id = this.data?.id! ?? 0;

      console.log(productData, this.authService.userId);

      this.productService
        .addOrEditProduct(productData, this.authService.userId)
        .subscribe(() => {
          this.toastr.success(
            this.isEditMode ? "Product updated" : "Produc added",
            "Success"
          );
          this.dialogRef.close(true);
        });
    } else {
      this.errorMessage = "Please fill in all required fields.";
    }
  }
}
