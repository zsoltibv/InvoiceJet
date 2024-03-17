import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
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
  isEditMode: boolean = false;

  constructor(private firmService: FirmService, private authService: AuthService, private snackBar: MatSnackBar, @Inject(MAT_DIALOG_DATA) public data: IFirm,
    private dialogRef: MatDialogRef<AddEditClientDialogComponent>) {
    this.firmDetailsForm = new FormGroup({
      firmName: new FormControl('', Validators.required),
      cuiValue: new FormControl('', Validators.required),
      regCom: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      county: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.isEditMode = true;
      this.firmDetailsForm.setValue({
        firmName: this.data.name || '',
        cuiValue: this.data.cui || '',
        regCom: this.data.regCom || '',
        address: this.data.address || '',
        county: this.data.county || '',
        city: this.data.city || '',
      });
    }
  }

  onSubmit(): void {
    if (this.firmDetailsForm.invalid) {
      return;
    }

    const firm: IFirm = {
      id: this.data?.id! ?? 0,
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
      this.firmService.addOrEditFirm(firm, this.authService.userId, false).subscribe({
        next: (response) => {
          this.snackBar.open('Firm details updated successfully', 'Close', {
            duration: 2000,
          });
          this.dialogRef.close(true);
        },
        error: (err) => {
          this.errorMessage = err.message;
        }
      });
    } else {
      this.errorMessage = 'Please fill all the required fields';
    }
  }

  onCloudIconClick(): void {
    console.log('Cloud icon clicked');
    this.firmService.getFirmFromAnaf(this.firmDetailsForm.value.cuiValue).subscribe({
      next: (firm) => {
        console.log(firm);
        this.firmDetailsForm.patchValue({
          firmName: firm.name,
          regCom: firm.regCom,
          address: firm.address,
          county: firm.county,
          city: firm.city
        });
      },
      error: (err) => {
        this.snackBar.open('Failed to retrieve firm data', 'Close', {
          duration: 2000,
        });
      }
    });
  }
}
