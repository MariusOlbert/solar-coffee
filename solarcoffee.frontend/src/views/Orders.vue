<template>
    <div>
        <h1 id="ordersTitle">Sales Orders</h1>
        <hr/>
        <table class="table" id="sales-orders" v-if="orders.length">
            <thead>
                <tr>
                    <th>CustomerId</th>
                    <th>OrderId</th>
                    <th>Order Total</th>
                    <th>Order Status</th>
                    <th>Mark Complete</th>
                </tr>
            </thead>
            <tr v-for="order in orders" :key="order.id">
                <td>
                    {{order.customer.id}}
                </td>
                <td>
                    {{order.id}}
                </td>
                <td>
                    {{getTotal(order) | price}}
                </td>
                <td :class="{green : order.isPaid}" >
                    {{ getStatus(order.isPaid)}}
                </td>
                <td>
                    <div v-if="!order.isPaid" class="lni lni-check-mark-circle order-complete" @click="markComplete(order.id)"></div>
                </td>
            </tr>
        </table>
    </div>
</template>
<script lang="ts">
import { Component, Vue} from 'vue-property-decorator';
import SolarButton from '@/components/SolarButton.vue';
import { OrderService } from '@/services/OrderService';
import { ISalesOrder } from '@/types/SalesOrder';
import { ILineItem } from '@/types/Invoice';

const orderService = new OrderService();

@Component({
    name: 'Orders',
    components: {SolarButton}
})
export default class Orders extends Vue {
    orders: ISalesOrder[] = [];
    
    getTotal(order: ISalesOrder) {
        return order.salesOrderItems?.reduce((a: any, b: ILineItem) => a + (b?.product!.price * b!.quantity), 0)
    }

    getStatus(isPaid: boolean) {
        return isPaid ? "Paid in Full" : "Unpaid";
    }

    async markComplete(orderId: number) {
        await orderService.markOrderComplete(orderId);
        this.fetchData();
    }

    async created() {
        this.fetchData();
    }
    async fetchData() {
        this.orders = await orderService.getOrders();
    }

}
</script>
<style scoped lang="scss">

</style>