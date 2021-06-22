<template>
    <div>
        <h1 id="invoiceTitle">
            Create Invoice
        </h1>
        <hr />
        <div class="invoice-step" v-if="invoiceStep === 1">
            <h2>Step 1: Select Customer</h2>
            <div class="invoice-step-detail" v-if="customers.length">
                <label for="customer">Customer:</label>
                <select name="customer" id="customer" v-model="selectedCustomerId" class="invoice-customers">
                    <option disabled value="">Please select a Customer</option>
                    <option v-for="c in customers" :value="c.id" :key="c.id">{{ c.firstName + " " + c.lastName}}</option>
                </select>
            </div>

        </div>
        <div class="invoice-step" v-if="invoiceStep === 2">
            <h2>Step 2: Create Order</h2>
            <div class="invoice-step-detail" v-if="inventory.length">
                <label for="product">Product</label>
                <select name="product" id="product" v-model="newItem.product" class="invoice-line-item">
                    <option disabled value="">Please select a Product</option>
                    <option v-for="i in inventory" :value="i.product" :key="i.product.id">
                        {{i.product.name}}
                    </option>
                </select>
                <label for="quantity">Quantity:</label>
                <input type="number" v-model="newItem.quantity" id="quantity" min="0" />
            </div>
            <div class="invoice-item-actions">
                <solar-button :disabled="!newItem.product || !newItem.quantity" @button:click="addLineItem">Add Line Item</solar-button>
                <solar-button :disabled="!lineItems.length" @button:click="finalizeOrder">Finalize Order</solar-button>
            </div>
        </div>
        <div class="invoice-step" v-if="invoiceStep === 3">
            <h2>Step 3: Review and Submit</h2>
            <solar-button @button:click="submitInvoice">Submit Invoice</solar-button>
            <hr/>
            <div class="invoice-step-detail" id="invoice" ref="invoice">
                <div class="invoice-logo">
                    <img src="../assets/images/coffee-cup.svg" id="imgLogo" alt="Solar Coffee logo" class="invoice-logo" />
                    <h3>1337 Solar Lane</h3>
                    <h3>San Somewhere, CA 90000</h3>
                    <h3>USA</h3>
                    <hr/>
                    <div class="invoice-order-line" v-if="lineItems.length">
                        <div class="invoice-header">
                            <h3>Invoice: {{ new Date() | humanizeDate }}</h3>
                            <h3>Customer: {{ this.selectedCustomer.firstName + " " + this.selectedCustomer.lastName}}</h3>
                            <h3>Address: {{ this.selectedCustomer.primaryAddress.addressLine1}}</h3>
                            <h3 v-if="this.selectedCustomer.primaryAddress.addressLine2">
                                {{this.selectedCustomer.primaryAddress.addressLine2}}
                            </h3>
                            <h3>
                                {{this.selectedCustomer.primaryAddress.city}}
                                {{this.selectedCustomer.primaryAddress.state}}
                                {{this.selectedCustomer.primaryAddress.postalCode}}
                            </h3>
                            <h3>{{this.selectedCustomer.primaryAddress.country}}</h3>
                        </div>
                        <table class="table" v-if="lineItems.length">
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>Description</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    <th>Subtotal</th>
                                </tr>
                            </thead>
                            <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
                                <td>{{ lineItem.product.name}}</td>
                                <td>{{ lineItem.product.description }}</td>
                                <td>{{ lineItem.quantity}}</td>
                                <td>{{lineItem.product.price}}</td>
                                <td>{{ lineItem.product.price * lineItem.quantity | price}}</td>
                            </tr>
                            <tr>
                                <th colspan="4"></th>
                                <th>Grand Total</th>
                            </tr>
                            <tfoot>
                                <tr>
                                    <td colspan="4" class="due">Balance due upon receipt:</td>
                                    <td class="price-final">{{ runningTotal | price}}</td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="invoice-step-actions">
            <solar-button @button:click="prev" :disabled="!canGoPrev">Previous</solar-button>
            <solar-button @button:click="next" :disabled="!canGoNext">Next</solar-button>
            <solar-button @button:click="startOver">Start Over</solar-button>
        </div>
        <div class="invoice-order-lineItems" v-if="lineItems.length">
            <h3>Runing Total:</h3>
            <hr/>
            {{ runningTotal | price }}
        </div>
        <hr />
        <table class="table" v-if="lineItems.length">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Description</th>
                    <th>Quantity</th>
                    <th>Price</th>
                    <th>Subtotal</th>
                </tr>
            </thead>
            <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
                <td>{{ lineItem.product.name}}</td>
                <td>{{ lineItem.product.description }}</td>
                <td>{{ lineItem.quantity}}</td>
                <td>{{lineItem.product.price}}</td>
                <td>{{ lineItem.product.price * lineItem.quantity | price}}</td>
            </tr>
        </table>
    </div>
</template>
<script lang="ts">
import { Component, Vue} from 'vue-property-decorator';
import SolarButton from '@/components/SolarButton.vue';
import {IInvoice, ILineItem} from '@/types/Invoice';
import {IProductInventory} from '@/types/Product';
import {ICustomer} from '@/types/Customer';
import CustomerService from '@/services/CustomerService';
import InventoryService from '@/services/InventoryService';
import InvoiceService from '@/services/InvoiceService';
import {jsPDF} from 'jspdf';
import html2canvas from 'html2canvas';


const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
    name: 'CreateInvoice',
    components: {SolarButton}
})
export default class CreateInvoice extends Vue {
    invoiceStep = 1;
    invoice: IInvoice = {
        createdOn: new Date(),
        customerId: 0,
        lineItems: [],
        updatedOn: new Date()
    };
    customers: ICustomer[] = [];
    selectedCustomerId: number = 0;
    inventory: IProductInventory[] = [];
    lineItems: ILineItem[] = [];
    newItem: ILineItem = {
        product: undefined,
        quantity: 0
    };

    get canGoNext() {
        if (this.invoiceStep === 1) {
            return this.selectedCustomerId !== 0;
        }
        if (this.invoiceStep === 2 && this.lineItems.length) {
            return true;
        }
        if (this.invoiceStep === 3) {
            return false;
        }

        return false;
    }


    get canGoPrev() {
        if (this.invoiceStep === 1) {
            return false;
        }
        return true;
    }

    get runningTotal() {
        return this.lineItems?.reduce((a, b) => a + (b?.product!.price * b!.quantity), 0);
    }

    get selectedCustomer() {
        return this.customers.find(c => c.id == this.selectedCustomerId)
    }

    async submitInvoice() : Promise<void> {
        this.invoice = {
            customerId: this.selectedCustomerId,
            lineItems: this.lineItems
        }

        await invoiceService.makeNewInvoice(this.invoice);

        this.dowloadPdf();
        await this.$router.push("/order");
    }

    dowloadPdf() {
        let pdf = new jsPDF("p", "pt", "a4", true);
        let invoice = document?.getElementById('invoice');
        let width : number = 595;
        let height : number = 842;

        if (invoice) {
            html2canvas(invoice).then(canvas => {
                let image = canvas.toDataURL('image/png');
                pdf.addImage(image, 'PNG', 0, 0, width, height);
                pdf.save('invoice');
            });
        }
    }

    async fetchData() : Promise<void> {
        //customerService.getCustomers().then(res => this.customers = res).catch(() => {});
        this.customers = await customerService.getCustomers();
        this.inventory = await inventoryService.getInventory();
    }

    async created() {
        await this.fetchData();
    }

    prev() : void {
        if (this.invoiceStep === 1) {
            return;
        }
        this.invoiceStep -= 1;
    }
    next() : void {
        if (this.invoiceStep === 3) {
            return;
        }
        this.invoiceStep += 1;
    }
    startOver() : void {    
        this.invoiceStep = 1;
        this.invoice = { customerId: 0, lineItems: [] }
        this.lineItems.length = 0;
    }
    finalizeOrder() {
        this.invoiceStep = 3;
    }
    addLineItem() {
         let newItem: ILineItem = {
             product: this.newItem.product,
             quantity: Number(this.newItem.quantity.toString())
         };
         let existingItems = this.lineItems.map(item => item?.product?.id);
         if (existingItems.includes(newItem?.product?.id)) {
             let lineItem = this.lineItems.find(item => item?.product?.id === newItem?.product?.id);
             lineItem!.quantity = Number(lineItem?.quantity?.toString()) + newItem.quantity;             
         }   else {
             this.lineItems.push(this.newItem);
         }
         this.newItem = { product: undefined, quantity: 0}
    }
}
</script>
<style scoped lang="scss">
@import '@/scss/global.scss';
.invoice-step {
    h2 {
        margin-bottom: 1rem;
    }
}

.invoice-step-detail {
    margin-left: 0.8rem;
}

.invoice-step-actions {
    display: flex;
    margin-bottom: 0.8rem;
}

.price-pre-tax, .due {
    font-weight: bold;
}

.price-final {
    font-weight: bold;
    color: $solar-green;
}

.invoice-header {
    width: 100%;
    margin-bottom: 1.2rem;
}

.invoice-logo {
    padding-top: 1.4rem;
    margin-bottom: 2rem;
    text-align: center;
    img {
        width: 120px;
    }
}
</style>