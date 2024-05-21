import { IDocumentProductRequest } from "./IDocumentProductRequest";
import { IDocumentSeries } from "./IDocumentSeries";
import { IFirm } from "./IFirm";

export interface IDocumentRequest {
  client: IFirm;
  documentNumber?: string;
  documentSeries: IDocumentSeries;
  dueDate: Date;
  issueDate: Date;
  products: IDocumentProductRequest[];
}
