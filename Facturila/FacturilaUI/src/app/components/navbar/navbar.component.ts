import { Component, ViewChild } from "@angular/core";
import { NavigationEnd, Router } from "@angular/router";
import { AuthService } from "src/app/services/auth.service";
import { SidebarService } from "src/app/services/sidebar.service";

@Component({
  selector: "app-navbar",
  templateUrl: "./navbar.component.html",
  styleUrls: ["./navbar.component.scss"],
})
export class NavbarComponent {
  isLoginOrRegister = false;

  constructor(
    private authService: AuthService,
    private sidebarService: SidebarService,
    private router: Router
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoginOrRegister = !event.url.includes('login') && !event.url.includes('register');
      }
    });
  }

  ngOnInit(): void {
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(["/login"]);
  }

  toggleSidebar(): void {
    this.sidebarService.toggleSidebar();
  }

  get userInfo(): any {
    return this.authService.userInfo;
  }
}
