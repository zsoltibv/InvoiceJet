import { AuthService } from './../../../services/auth.service';
import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from "@angular/forms";
import { UserRegister } from "src/app/models/user-register";
import { Router } from '@angular/router';
import { distinctUntilChanged } from "rxjs";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {
  isFormValid = true;

  readonly userLoginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, passwordValidator]),
  });

  constructor(private authService: AuthService, private router: Router) {
    this.userLoginForm.valueChanges.pipe(distinctUntilChanged()).subscribe(() => {
      this.userLoginForm.markAsTouched();
    });
  }

  login() {
    const user: UserRegister = Object.assign({
      email: this.userLoginForm.value.email,
      password: this.userLoginForm.value.password,
    });

    this.authService.login(user).subscribe({
      next: (token: string) => {
        this.isFormValid = true;
        localStorage.setItem('authToken', token);
        this.router.navigateByUrl('dashboard');
      },
      error: () => {
        this.isFormValid = false;
      }
    });
  }
}

const latinChars = /^[a-zA-Z]+$/;

export function passwordValidator(field: AbstractControl): Validators | null {
  return field.value && latinChars.test(field.value)
    ? null
    : {
      other: 'Only latin letters are allowed',
    };
}
