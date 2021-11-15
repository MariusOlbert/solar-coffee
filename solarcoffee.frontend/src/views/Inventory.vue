<template>
    <div class="inventory-container">
        <h1 id="inventoryTitle">Inventory Dashboard</h1>
        <hr/>
        <inventory-chart />
        <div class="inventory-actions">
            <solar-button @button:click="showProductModal" id="addNewBtn">
                Add new Item
            </solar-button>
            <solar-button @button:click="showShipmentModal" id="receiveShipmentBtn">
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
                <td v-bind:class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">
                    {{item.quantityOnHand}}
                </td>
                <td>                    
                    {{item.product.price | price}}
                </td>
                <td>
                    <span v-if="item.product.isTaxable">Yes</span>
                    <span v-else>No</span>
                </td>
                <td>
                    <div class="lni lni-cross-circle product-archive" @click="archiveProduct(item.product.id)"></div>        
                </td>
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
    import ProductService from '@/services/ProductService';
    import InventoryChart from "@/components/charts/InventoryChart.vue";

    const inventoryService = new InventoryService();
    const productService = new ProductService();

    @Component({
        name: 'Inventory',
        components: {InventoryChart, SolarButton, NewProductModal, ShipmentModal}
    })
    export default class Inventory extends Vue {
        isProductVisible : boolean = false;
        isShipmentVisible : boolean = false;
        inventory: IProductInventory [] = [];

        applyColor(current: number, target: number) {
            if (current <= 0) {
                return "red";
            }
            if(Math.abs(target - current) > 8) {
                return "yellow";
            }
            return "green";
        }

       async archiveProduct(productId: number) {
           await productService.archive(productId);
           await this.fetchData();
        }

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
        async saveNewProduct(newProduct: IProduct) {
            await productService.save(newProduct);
            this.isProductVisible = false;
            await this.fetchData();
        }
        async saveNewShipment(shipment: IShipment) {
            await inventoryService.updateInventoryQuantity(shipment);
            this.isShipmentVisible = false;
            await this.fetchData();
        }

        async fetchData() {
            this.inventory = await inventoryService.getInventory();
            await this.$store.dispatch("assignSnapshots");
        }

        async created() {
            await this.fetchData();
        }
    }
</script>

<style scoped lang="scss">
    @import "@/scss/global.scss";
    .inventory-actions {
        display: flex;
        margin-bottom: 0.8rem;
    }
    .product-archive {
        cursor: pointer;
        font-weight: bold;
        font-size: 1.2rem;
        color: $solar-red;
    }
</style>
