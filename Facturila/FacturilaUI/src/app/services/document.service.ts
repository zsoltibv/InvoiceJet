import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "environment";
import { IDocumentAutofill } from "../models/IDocumentAutofill";
import { IDocumentRequest } from "../models/IDocumentRequest";
import { IDocumentTableRecord } from "../models/IDocumentTableRecord";

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

  addOrEditDocument(documentData: IDocumentRequest) {
    return this.http.post(
      `${this.baseUrl}/Document/AddOrEditDocument`,
      documentData
    );
  }

  generateDocumentPdf(documentData: IDocumentRequest) {
    return this.http.post(
      `${this.baseUrl}/Document/GenerateDocumentPdf`,
      documentData
    );
  }

  getGeneratedDocumentPdf(documentData: IDocumentRequest) {
    return this.http.post(
      `${this.baseUrl}/Document/GetInvoicePdfStream/`,
      documentData,
      {
        responseType: "blob",
      }
    );
  }

  getDocuments(documentTypeId: number) {
    return this.http.get<IDocumentTableRecord[]>(
      `${this.baseUrl}/Document/GetDocumentTableRecords/${documentTypeId}`
    );
  }

  getDocumentById(documentId: number) {
    return this.http.get<IDocumentRequest>(
      `${this.baseUrl}/Document/GetDocumentById/${documentId}`
    );
  }
}
