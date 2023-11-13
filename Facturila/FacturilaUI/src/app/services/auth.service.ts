import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { UserLogin } from "../models/user-login";
import { UserRegister } from "../models/user-register";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  private apiEndpoint: string = 'https://localhost:7229/Auth';
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };

  public register(user: UserRegister): Observable<any> {
    return this.http.post<any>(
      this.apiEndpoint + '/register',
      user,
      this.options
    );
  }

  public login(user: UserLogin): Observable<string> {
    return this.http.post(
      this.apiEndpoint + '/login', user, {
      responseType: 'text'
    })
  }

  public getUserData(): any {

  }
}
