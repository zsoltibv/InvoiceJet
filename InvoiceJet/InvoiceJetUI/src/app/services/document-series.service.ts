import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "environment";
import { IDocumentSeries } from "../models/IDocumentSeries";

@Injectable({
  providedIn: "root",
})
export class DocumentSeriesService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  public getDocumentSeriesForUserId() {
    return this.http.get<IDocumentSeries[]>(
      `${this.baseUrl}/DocumentSeries/GetAllDocumentSeriesForUserId`
    );
  }
}
