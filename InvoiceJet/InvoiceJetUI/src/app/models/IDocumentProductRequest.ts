import { IProduct } from "./IProduct";

export interface IDocumentProductRequest {
  id?: 0;
  name: string;
  unitPrice: number;
  totalPrice: number;
  containsTVA: boolean;
  unitOfMeasurement: string;
  tvaValue: number;
  quantity: number;
}
