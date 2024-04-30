import { IProduct } from "./IProduct";

export interface IDocumentProductRequest {
  name: string;
  unitPrice: number;
  totalPrice: number;
  containsTVA: boolean;
  unitOfMeasurement: string;
  tvaValue: number;
  quantity: number;
}
