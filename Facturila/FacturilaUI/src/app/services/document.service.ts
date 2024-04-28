import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "environment";
import { IDocumentAutofill } from "../models/IDocumentAutofill";

@Injectable({
  providedIn: "root",
})
export class DocumentService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  public getDocumentSeriesForUserId(userId: string, documentTypeId: number) {
    return this.http.get<IDocumentAutofill>(
      `${this.baseUrl}/Document/GetDocumentAutofillInfo/${userId}/${documentTypeId}`
    );
  }
}
