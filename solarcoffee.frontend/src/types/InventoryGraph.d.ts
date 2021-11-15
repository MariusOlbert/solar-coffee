export interface IInventoryTimeline {
    timeline: Date[],
    productInventorySnapshots: ISnapshot[];
}

export interface ISnapshot {
    productId: number,
    quantityOnHand: number[];
}