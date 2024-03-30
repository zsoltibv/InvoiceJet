import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { JwtHelperService, JWT_OPTIONS } from "@auth0/angular-jwt";
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { RegisterComponent } from './components/register/register.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { FirmDetailsComponent } from './components/firm/firm-details/firm-details.component';
import { MaterialModule } from "./material/material.module";
import { AuthInterceptor } from "./services/interceptor/auth.interceptor";
import { ClientsComponent } from './components/firm/clients/clients.component';
import { AddEditClientDialogComponent } from './components/firm/add-edit-client-dialog/add-edit-client-dialog.component';
import { BankAccountsComponent } from './components/firm/bank-accounts/bank-accounts.component';
import { AddOrEditBankAccountDialogComponent } from './components/firm/bank-accounts/add-or-edit-bank-account-dialog/add-or-edit-bank-account-dialog.component';
import { ProductsComponent } from './components/products/products.component';
import { AddOrEditProductDialogComponent } from './components/products/add-or-edit-product-dialog/add-or-edit-product-dialog.component';
import { DocumentSeriesComponent } from './components/document-series/document-series.component';
import { AddOrEditDocumentSeriesDialogComponent } from './components/document-series/add-or-edit-document-series-dialog/add-or-edit-document-series-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    DashboardComponent,
    RegisterComponent,
    SidebarComponent,
    FirmDetailsComponent,
    ClientsComponent,
    AddEditClientDialogComponent,
    BankAccountsComponent,
    AddOrEditBankAccountDialogComponent,
    ProductsComponent,
    AddOrEditProductDialogComponent,
    DocumentSeriesComponent,
    AddOrEditDocumentSeriesDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    HttpClientModule
  ],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    JwtHelperService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
