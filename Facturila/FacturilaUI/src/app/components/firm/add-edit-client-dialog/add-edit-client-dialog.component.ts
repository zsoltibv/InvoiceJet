import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material/snack-bar";
import { IFirm } from "src/app/models/IFirm";
import { AuthService } from "src/app/services/auth.service";
import { FirmService } from "src/app/services/firm.service";

@Component({
  selector: 'app-add-edit-client-dialog',
  templateUrl: './add-edit-client-dialog.component.html',
  styleUrls: ['./add-edit-client-dialog.component.scss']
})
export class AddEditClientDialogComponent {
  firmDetailsForm: FormGroup;
  initialFormValues: any;
  currentUserFirm!: IFirm;
  errorMessage: string | null = null;

  constructor(private firmService: FirmService, private authService: AuthService, private snackBar: MatSnackBar) {
    this.firmDetailsForm = new FormGroup({
      firmName: new FormControl('', Validators.required),
      cuiValue: new FormControl('', Validators.required),
      regCom: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      county: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required)
    });
  }

  onSubmit(): void {
    if (this.firmDetailsForm.invalid) {
      return;
    }

    const firm: IFirm = {
      id: this.currentUserFirm?.id! ?? 0,
      name: this.firmDetailsForm.value.firmName!,
      cui: this.firmDetailsForm.value.cuiValue!,
      regCom: this.firmDetailsForm.value.regCom!,
      address: this.firmDetailsForm.value.address!,
      county: this.firmDetailsForm.value.county!,
      city: this.firmDetailsForm.value.city!
    };

    console.log(firm);

    if (this.firmDetailsForm.valid) {
      console.log(this.authService.userId);
      this.firmService.addOrEditFirm(firm, this.authService.userId).subscribe({
        next: (response) => {
          this.snackBar.open('Firm added successfully', 'Close', {
            duration: 2000,
          });
        },
        error: (err) => {
          this.errorMessage = err.message;
        }
      });
    } else {
      this.errorMessage = 'Please fill all the required fields';
    }
  }
}
