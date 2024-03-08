import { environment } from './../../../environment';
import { Injectable } from '@angular/core';
import { IRegisterUser } from "../models/IRegisterUser";
import { Observable } from "rxjs";
import { ILoginUser } from "../models/ILoginUser";
import { HttpClient, HttpEvent } from "@angular/common/http";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  private baseUrl = environment.apiUrl;
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };

  public register(user: IRegisterUser): Observable<HttpEvent<string>> {
    return this.http.post<string>(
      this.baseUrl + '/Auth/register',
      user,
      this.options
    );
  }

  public login(user: ILoginUser): Observable<string> {
    return this.http.post(
      this.baseUrl + '/Auth/login',
      user,
      {
        responseType: 'text'
      }
    );
  }

  public logout(): void {
    localStorage.removeItem('authToken');
  }

  public isLoggedIn(): boolean {
    const token = this.authToken;
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  get userEmail(): string | null {
    const token = this.authToken;

    if (token) {
      const decodedToken: any = this.jwtHelper.decodeToken(token);
      return decodedToken.email || null;
    }

    return null;
  }

  get authToken(): string | null {
    return localStorage.getItem('authToken');
  }
}
