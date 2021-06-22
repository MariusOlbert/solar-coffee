import { IProduct } from "./Product";

export interface IInvoice {
    createdOn?: Date;
    updatedOn?: Date;
    customerId: number;
    lineItems?: ILineItem[];
}

export interface ILineItem {
    product?: IProduct;
    quantity: number;
}
