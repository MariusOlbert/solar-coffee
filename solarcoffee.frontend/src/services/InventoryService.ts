import axios from 'axios';

/**
 * Inventory
 * Provides UI business logic associated with product Inventory
 */

export default class InventoryService {
    API_URL = process.env.VUE_APP_API_URL;
    
    public async getInventory() : Promise<any> {
        console.log('get invenotory', this.API_URL);
        axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
        let result: any = await axios.get(`${this.API_URL}/inventory`, {
            headers: {}
        });
    }    
}
