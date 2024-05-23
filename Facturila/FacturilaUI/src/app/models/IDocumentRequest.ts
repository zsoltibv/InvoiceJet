import { IDocumentProductRequest } from "./IDocumentProductRequest";
import { IDocumentSeries } from "./IDocumentSeries";
import { IDocumentType } from "./IDocumentType";
import { IFirm } from "./IFirm";

export interface IDocumentRequest {
  client: IFirm;
  documentNumber?: string;
  documentSeries: IDocumentSeries;
  documentType: IDocumentType;
  dueDate: Date;
  issueDate: Date;
  products: IDocumentProductRequest[];
}
