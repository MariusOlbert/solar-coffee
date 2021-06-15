<template></template>
<script lang="ts">
import { Component, Vue} from 'vue-property-decorator';
import SolarButton from '@/components/SolarButton.vue';

@Component({
    name: 'Create Invoice',
    components: {SolarButton}
})
export default class CreateInvoice extends Vue {}
</script>
<style scoped lang="scss">

</style>