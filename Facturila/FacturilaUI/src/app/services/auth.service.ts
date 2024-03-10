import { UserService } from './user.service';
import { environment } from './../../../environment';
import { Injectable } from '@angular/core';
import { IRegisterUser } from "../models/IRegisterUser";
import { BehaviorSubject, Observable, map } from "rxjs";
import { ILoginUser } from "../models/ILoginUser";
import { HttpClient, HttpEvent } from "@angular/common/http";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  loggedInUser: IRegisterUser = {} as IRegisterUser;
  decodedToken: any;

  private baseUrl = environment.apiUrl;
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };

  private loggedInUserSubject = new BehaviorSubject<IRegisterUser | null>(null);
  public loggedInUser$: Observable<IRegisterUser | null> = this.loggedInUserSubject.asObservable();

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, public userService: UserService) {
    const token = this.authToken;
    if (token) {
      this.decodedToken = this.jwtHelper.decodeToken(token);
      this.userService.getUserByEmail(this.decodedToken.email).subscribe(user => {
        this.loggedInUser = user;
        this.loggedInUserSubject.next(user);
      });
    }
  }

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

  get authToken(): string | null {
    return localStorage.getItem('authToken');
  }

  get userId(): string {
    return this.decodedToken.userId;
  }

  get nameInitials$(): Observable<string> {
    return this.loggedInUser$.pipe(
      map(user => user ? `${user.firstName[0]}${user.lastName[0]}` : 'N/A')
    );
  }
}
