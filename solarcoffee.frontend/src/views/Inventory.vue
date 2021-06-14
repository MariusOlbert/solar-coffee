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

    @Component({
        name: 'Inventory',
        components: {SolarButton, NewProductModal, ShipmentModal}
    })
    export default class Inventory extends Vue {
        isProductVisible : boolean = false;
        isShipmentVisible : boolean = false;
        inventory: IProductInventory [] = [
            {
                id: 1,
                product: {
                    id: 1,
                    name: 'Some Product',
                    description: 'Good stuff',
                    price: 100,
                    createdOn: new Date(),
                    updatedOn: new Date(),
                    isTaxable: true,
                    isArchived: false
                },
                quantityOnHand: 100,
                idealQuantity: 100
            },
            {
                id: 2,
                product: {
                    id: 2,
                    name: 'Some Product 2',
                    description: 'Good stuff',
                    price: 200,
                    createdOn: new Date(),
                    updatedOn: new Date(),
                    isTaxable: false,
                    isArchived: false
                },
                quantityOnHand: 40,
                idealQuantity: 20
            },
        ];
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
    }
</script>
<style scoped lang="scss">
</style>
