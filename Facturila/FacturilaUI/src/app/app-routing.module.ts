import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { LoginComponent } from "./components/login/login.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { AuthGuard } from "./guards/auth.guard";
import { RegisterComponent } from "./components/register/register.component";
import { FirmDetailsComponent } from "./components/firm/firm-details/firm-details.component";
import { AuthService } from "./services/auth.service";
import { ClientsComponent } from "./components/firm/clients/clients.component";
import { BankAccountsComponent } from "./components/firm/bank-accounts/bank-accounts.component";
import { ProductsComponent } from "./components/products/products.component";

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'dashboard/firm-details', component: FirmDetailsComponent, canActivate: [AuthGuard] },
  { path: 'dashboard/clients', component: ClientsComponent, canActivate: [AuthGuard] },
  { path: 'dashboard/bank-accounts', component: BankAccountsComponent, canActivate: [AuthGuard] },
  { path: 'dashboard/products', component: ProductsComponent, canActivate: [AuthGuard] },
  { path: '**', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
  constructor(private router: Router, private authService: AuthService) {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/dashboard']);
    }
  }
}
