import { Injectable } from '@angular/core';
import { RegisterUser } from "../models/RegisterUser";
import { Observable } from "rxjs";
import { LoginUser } from "../models/LoginUser";
import { HttpClient, HttpEvent } from "@angular/common/http";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  private apiEndpoint: string = 'https://localhost:7229/Auth';
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };

  public register(user: RegisterUser): Observable<HttpEvent<string>> {
    return this.http.post<string>(
      this.apiEndpoint + '/register',
      user,
      this.options
    );
  }

  public login(user: LoginUser): Observable<string> {
    return this.http.post(
      this.apiEndpoint + '/login',
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
