import { Component, ViewChild } from '@angular/core';
import { Router } from "@angular/router";
import { MenuItem } from "primeng/api";
import { AuthService } from "src/app/services/auth.service";
import { SidebarService } from "src/app/services/sidebar.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  menuItems: MenuItem[] = [];

  selectedOption: any;
  dropdownOptions: any[] = [
    {
      label: 'Logout', value: 'logout', icon: 'pi pi-sign-out', command: () => {
        this.logout();
      }
    },
  ];

  constructor(private authService: AuthService, private router: Router, private sidebarService: SidebarService) { }

  ngOnInit(): void {
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
    localStorage.removeItem('authToken');
    this.router.navigateByUrl('login');
  }

  toggleSidebar(): void {
    this.sidebarService.toggleSidebar();
  }
}
