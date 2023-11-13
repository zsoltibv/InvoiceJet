import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginFormComponent } from './components/auth/login-form/login-form.component';
import { RegisterFormComponent } from './components/auth/register-form/register-form.component';
import { ReactiveFormsModule } from "@angular/forms";
import { FormsModule } from "@angular/forms";

import {
  TuiRootModule,
  TuiDialogModule,
  TuiAlertModule,
  TuiButtonModule,
  TuiErrorModule,
  TuiScrollbarModule,
  TuiTextfieldControllerModule,
  TuiLabelModule,
} from "@taiga-ui/core";
import { TuiInputModule, TuiInputPasswordModule, TuiFieldErrorPipeModule } from "@taiga-ui/kit";
import { AuthInterceptor } from "./services/interceptor/auth.interceptor";
import { ErrorInterceptor } from "./services/interceptor/error.interceptor";
import { DashboardComponent } from './components/dashboard/dashboard/dashboard.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/dashboard/sidebar/sidebar.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    RegisterFormComponent,
    DashboardComponent,
    NavbarComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    // tui imports
    TuiRootModule,
    TuiDialogModule,
    TuiAlertModule,
    TuiInputModule,
    TuiInputPasswordModule,
    TuiButtonModule,
    TuiErrorModule,
    TuiScrollbarModule,
    TuiFieldErrorPipeModule,
    TuiTextfieldControllerModule,
    TuiLabelModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  }, {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true,
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
