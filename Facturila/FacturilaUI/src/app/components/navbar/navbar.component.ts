import { Component, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "src/app/services/auth.service";
import { SidebarService } from "src/app/services/sidebar.service";

@Component({
  selector: "app-navbar",
  templateUrl: "./navbar.component.html",
  styleUrls: ["./navbar.component.scss"],
})
export class NavbarComponent {
  constructor(
    private authService: AuthService,
    private sidebarService: SidebarService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  public isLoginOrRegister(): boolean {
    const currentUrl = this.router.url;
    return currentUrl.includes("/login") || currentUrl.includes("/register");
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
