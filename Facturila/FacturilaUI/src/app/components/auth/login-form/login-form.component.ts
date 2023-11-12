import { UserLogin } from "src/app/models/user-login";
import { AuthService } from './../../../services/auth.service';
import { Component } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { UserRegister } from "src/app/models/user-register";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {
  constructor(private authService: AuthService) { }

  login() {
    const user: UserRegister = Object.assign({
      email: this.userLoginForm.value.email,
      password: this.userLoginForm.value.password,
    });

    this.authService.login(user).subscribe((token: string) => {
      localStorage.setItem('authToken', token);
    });
  }

  userLoginForm = new FormGroup({
    email: new FormControl('john.doe.gmail.com'),
    password: new FormControl('hehe'),
  });
}
