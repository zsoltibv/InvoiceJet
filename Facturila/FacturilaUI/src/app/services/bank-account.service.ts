import { Injectable } from '@angular/core';
import { IBankAccount } from "../models/IBankAccount";
import { HttpClient } from "@angular/common/http";
import { environment } from "environment";

@Injectable({
  providedIn: 'root'
})
export class BankAccountService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUserFirmBankAccounts(userId: string) {
    return this.http.get<IBankAccount[]>(
      `${this.baseUrl}/BankAccount/GetUserFirmBankAccounts/${userId}`
    );
  }

  addOrEditBankAccount(bankAccount: IBankAccount, userId: string) {
    return this.http.put<IBankAccount>(
      `${this.baseUrl}/BankAccount/AddOrEditBankAccount/${userId}`,
      bankAccount
    );
  }
}
