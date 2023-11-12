import { Component } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { UserRegister } from "src/app/models/user-register";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent {
  constructor(private authService: AuthService) { }

  register() {
    const user: UserRegister = Object.assign({
      firstName: this.userRegisterForm.value.firstName,
      lastName: this.userRegisterForm.value.lastName,
      email: this.userRegisterForm.value.email,
      password: this.userRegisterForm.value.password,
    });

    this.authService.register(user).subscribe();
  }

  userRegisterForm = new FormGroup({
    firstName: new FormControl('John'),
    lastName: new FormControl('Doe'),
    email: new FormControl('john.doe.gmail.com'),
    password: new FormControl('hehe'),
  });
}
