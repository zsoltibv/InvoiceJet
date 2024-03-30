import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProduct } from "../models/IProduct";
import { environment } from "environment";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getProductsForUserId(userId: string) {
    return this.http.get<IProduct[]>(
      `${this.baseUrl}/Product/GetAllProductsForUserId/${userId}`
    );
  }

  public addOrEditProduct(product: IProduct, userId: string) {
    return this.http.put<IProduct>(
      `${this.baseUrl}/Product/AddOrEditProduct/${userId}`,
      product
    );
  }
}
