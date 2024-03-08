import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-firm-details',
  templateUrl: './firm-details.component.html',
  styleUrls: ['./firm-details.component.scss']
})
export class FirmDetailsComponent implements OnInit {
  registrationForm: FormGroup;
  errorMessage: string | null = null;

  constructor() {
    this.registrationForm = new FormGroup({
      firmName: new FormControl('', Validators.required),
      cuiValue: new FormControl('', Validators.required),
      regCom: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {

  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      console.log('Form Value', this.registrationForm.value);
      // Here you can call a service to send the form data to your server
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
