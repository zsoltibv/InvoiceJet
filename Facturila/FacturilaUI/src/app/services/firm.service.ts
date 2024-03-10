import { Injectable } from '@angular/core';
import { environment } from "environment";
import { IFirm } from "../models/IFirm";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class FirmService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public addOrEditFirm(firm: IFirm, userId: string, isClient: boolean = true) {
    return this.http.put<IFirm>(
      `${this.baseUrl}/Firm/addOrEditFirm/${userId}/${isClient}`,
      firm
    );
  }

  public getUsersFirm(userId: string) {
    return this.http.put<IFirm>(
      `${this.baseUrl}/Firm/GetUserFirmByUserId/${userId}`,
      userId
    );
  }
}
