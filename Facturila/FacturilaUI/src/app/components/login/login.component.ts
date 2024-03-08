import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LoginUser } from "src/app/models/LoginUser";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  errorMessage: string | null = null;
  hide = true;

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
  });

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const user: LoginUser = {
        email: this.loginForm.value.email!,
        password: this.loginForm.value.password!,
      };

      this.authService.login(user).subscribe({
        next: (token: string) => {
          localStorage.setItem('authToken', token);
          this.router.navigateByUrl('dashboard');
        },
        error: (err) => {
          console.log(err);
          this.errorMessage = err.error;
        }
      });
    }
  }
}
