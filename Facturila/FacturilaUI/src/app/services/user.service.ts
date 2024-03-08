import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { environment } from "environment";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.apiUrl;
  private options: any = {
    observe: 'response',
    responseType: 'text',
  };
}
