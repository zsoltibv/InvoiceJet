import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  private apiEndpoint: string = 'https://localhost:7229/User';
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };
}
