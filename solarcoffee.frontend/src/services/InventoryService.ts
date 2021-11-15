import axios from 'axios';
import { IProductInventory } from '@/types/Product';
import { IShipment } from '@/types/Shipment';
import {IInventoryTimeline} from "@/types/InventoryGraph";

/**
 * Inventory
 * Provides UI business logic associated with product Inventory
 */

export default class InventoryService {
    API_URL = process.env.VUE_APP_API_URL;
    
    public async getInventory() : Promise<IProductInventory[]> {           
        let result: any = await axios.get(`${this.API_URL}/inventory/`);
        console.log("inventory",result.data)
        return result.data;
    }
    
    public async updateInventoryQuantity(shipment: IShipment) {
        let result = await axios.patch(`${this.API_URL}/inventory/`, shipment);
        return result.data;
    }
    
    public async getSnapshotHistory(): Promise<IInventoryTimeline> {
        let result: any = await axios.get(`${this.API_URL}/inventory/snapshot`);
        console.log("snapshot history", result.data)
        return result.data;
    }
}
