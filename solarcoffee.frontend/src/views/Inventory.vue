<template>
    <div class="inventory-container">
        <h1 id="inventoryTitle">Inventory Dashboard</h1>
        <hr/>
        <div class="inventory-actions">
            <solar-button @click.native="showProductModal" id="addNewBtn">
                Add new Item
            </solar-button>
            <solar-button @click.native="showShipmentModal" id="receiveShipmentBtn">
                Receive Shipment
            </solar-button>
        </div>
        <table id="inventoryTable" class="table">
            <tr>
                <th>Item</th>
                <th>Quantity On-hand</th>
                <th>Unit Price</th>
                <th>Taxable</th>
                <th>Delete</th>
            </tr>
            <tr v-for="item in inventory" :key="item.id">
                <td>
                    {{ item.product.name}}
                </td>
                <td>
                    {{item.quantityOnHand}}
                </td>
                <td>                    
                    {{item.product.price | price}}
                </td>
                <td>
                    <span v-if="item.product.isTaxable">Yes</span>
                    <span v-else>No</span>
                </td>
                <td></td>
            </tr>
        </table>
        <new-product-modal v-if="isProductVisible"  @close="closeModals" @save:product="saveNewProduct" />
        <shipment-modal v-if="isShipmentVisible" :inventory="inventory" @close="closeModals" @save:shipment="saveNewShipment"/>
    </div>
</template>
<script lang="ts">
    import { Component, Vue} from 'vue-property-decorator';
    import { IProduct, IProductInventory } from '@/types/Product';
    import SolarButton from '@/components/SolarButton.vue';
    import NewProductModal from '@/components/modals/NewProductModal.vue';
    import ShipmentModal from '@/components/modals/ShipmentModal.vue';
    import { IShipment } from '@/types/Shipment';
    import InventoryService from '@/services/InventoryService';


    const inventoryService = new InventoryService();

    @Component({
        name: 'Inventory',
        components: {SolarButton, NewProductModal, ShipmentModal}
    })
    export default class Inventory extends Vue {
        isProductVisible : boolean = false;
        isShipmentVisible : boolean = false;
        inventory: IProductInventory [] = [];
        showProductModal() {
            this.isProductVisible = true;
            this.isShipmentVisible = false;
        };
        showShipmentModal() {
            this.isShipmentVisible = true;
            this.isProductVisible = false;
        };
        closeModals() {
            this.isShipmentVisible = false;
            this.isProductVisible = false;
        }
        saveNewProduct(newProduct: IProduct) {
            console.log('saveNewProduct', newProduct);
        }
        saveNewShipment(shipment: IShipment) {
            console.log('saveNewShipment', shipment);
        }

        async fetchData() {
            this.inventory = await inventoryService.getInventory();
        }

        async created() {
            await this.fetchData();
        }
    }
</script>

<style scoped lang="scss">

</style>
