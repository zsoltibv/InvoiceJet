import { AuthService } from './../../../services/auth.service';
import { FirmService } from './../../../services/firm.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from "@angular/material/snack-bar";
import { IFirm } from "src/app/models/IFirm";

@Component({
  selector: 'app-firm-details',
  templateUrl: './firm-details.component.html',
  styleUrls: ['./firm-details.component.scss']
})
export class FirmDetailsComponent implements OnInit {
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

  ngOnInit(): void {
    this.firmService.getUserActiveFirmById(this.authService.userId).subscribe({
      next: (firm) => {
        if (firm) {
          this.currentUserFirm = firm;
          this.firmDetailsForm.patchValue({
            firmName: firm.name,
            cuiValue: firm.cui,
            regCom: firm.regCom,
            address: firm.address,
            county: firm.county,
            city: firm.city
          });
        }
      },
      error: (err) => {
        console.log('Error', err);
      }
    });

    this.initialFormValues = this.firmDetailsForm.value;
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
      this.firmService.addOrEditFirm(firm, this.authService.userId, false).subscribe({
        next: (response) => {
          this.snackBar.open('Firm details updated successfully', 'Close', {
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

  onCloudIconClick(): void {
    console.log('Cloud icon clicked');
  }

  isFormChanged(): boolean {
    return JSON.stringify(this.initialFormValues.value) !== JSON.stringify(this.initialFormValues);
  }

  addNewClient() {
    console.log('Add new client');
  }
}
