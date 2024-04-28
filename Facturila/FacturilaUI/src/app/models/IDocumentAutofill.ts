import { IDocumentSeries } from "./IDocumentSeries";
import { IFirm } from "./IFirm";
import { IProduct } from "./IProduct";

export interface IDocumentAutofill {
  clients: IFirm[];
  documentSeries: IDocumentSeries[];
  products: IProduct[];
}
