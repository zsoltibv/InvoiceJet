import { AuthService } from './../../../services/auth.service';
import { FirmService } from './../../../services/firm.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IFirm } from "src/app/models/IFirm";

@Component({
  selector: 'app-firm-details',
  templateUrl: './firm-details.component.html',
  styleUrls: ['./firm-details.component.scss']
})
export class FirmDetailsComponent implements OnInit {
  firmDetailsForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private firmService: FirmService, private authService: AuthService) {
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
    // this.firmService.getUsersFirm(this.authService.userId).subscribe({

    // });
  }

  onSubmit(): void {
    if (this.firmDetailsForm.invalid) {
      return;
    }

    const firm: IFirm = {
      id: 0,
      name: this.firmDetailsForm.value.firmName!,
      CUI: this.firmDetailsForm.value.cuiValue!,
      regCom: this.firmDetailsForm.value.regCom!,
      address: this.firmDetailsForm.value.address!,
      county: this.firmDetailsForm.value.county!,
      city: this.firmDetailsForm.value.city!
    };

    if (this.firmDetailsForm.valid) {
      this.firmService.addOrEditFirm(firm, this.authService.userId.toString(), false).subscribe({
        next: (response) => {
          console.log('Response', response);
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
    // Logic to handle the cloud icon click
    console.log('Cloud icon clicked');
    // For example, you might want to open a file upload dialog here
  }
}
