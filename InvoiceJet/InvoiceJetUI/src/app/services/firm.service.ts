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

  public getUserActiveFirmById(userId: string) {
    return this.http.get<IFirm>(
      `${this.baseUrl}/Firm/GetUserActiveFirmById/${userId}`
    );
  }

  public getUserClientFirmsById(userId: string) {
    return this.http.get<IFirm[]>(
      `${this.baseUrl}/Firm/GetUserClientFirmsById/${userId}`
    );
  }

  public getFirmFromAnaf(cuiValue: string) {
    return this.http.get<IFirm>(
      `${this.baseUrl}/Firm/fromAnaf/${cuiValue}`
    );
  }
}
