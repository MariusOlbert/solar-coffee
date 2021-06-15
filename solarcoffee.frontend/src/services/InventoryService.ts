import { IProductInventory } from '@/types/Product';
import { IShipment } from '@/types/Shipment';
import axios from 'axios';

/**
 * Inventory
 * Provides UI business logic associated with product Inventory
 */

export default class InventoryService {
    API_URL = process.env.VUE_APP_API_URL;
    
    public async getInventory() : Promise<IProductInventory[]> {           
        let result = await axios.get(`${this.API_URL}/inventory/`);
        return result.data;
    }
    
    public async updateInventoryQuantity(shipment: IShipment) {
        let result = await axios.patch(`${this.API_URL}/inventory/`, shipment);
        return result.data;
    }
}
