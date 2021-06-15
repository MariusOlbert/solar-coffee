<template>
    <solar-modal>
        <template v-slot:header>Create New Customer</template>
    </solar-modal>
</template>
<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import SolarButton from '@/components/SolarButton.vue';
    import SolarModal from '@/components/modals/SolarModal.vue';

    @Component({
        name: "NewCustomerModal",
        components: { SolarButton, SolarModal }
    })

    export default class NewCustomerModal extends Vue {

    }
</script>
<style scoped lang="scss"></style>