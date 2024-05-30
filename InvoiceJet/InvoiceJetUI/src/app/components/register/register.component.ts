import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ILoginUser } from "src/app/models/ILoginUser";
import { IRegisterUser } from "src/app/models/IRegisterUser";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errorMessage: string | null = null;
  hide = true; // Use this to toggle password visibility

  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private authService: AuthService) { }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }

    const user: IRegisterUser = {
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!,
    };

    this.authService.register(user).subscribe({
      next: (response) => {
        // Handle response
      },
      error: (err) => {
        this.errorMessage = err.message;
      }
    });
  }
}
