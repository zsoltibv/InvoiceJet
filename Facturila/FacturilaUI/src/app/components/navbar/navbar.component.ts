import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { MenuItem } from "primeng/api";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  menuItems: MenuItem[] = [];

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.menuItems = [
      {
        label: 'Dashboard',
        routerLink: '/dashboard'
      },
    ];
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
    localStorage.removeItem('authToken');
    this.router.navigateByUrl('login');
  }
}
