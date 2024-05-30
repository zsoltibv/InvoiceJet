export interface IProduct {
  id?: number;
  name: string;
  price: number;
  containsTVA: boolean;
  unitOfMeasurement: string;
  tvaValue: number;
}
