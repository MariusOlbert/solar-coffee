<template></template>
<script lang="ts">
import { Component, Vue} from 'vue-property-decorator';
import SolarButton from '@/components/SolarButton.vue';

@Component({
    name: 'Orders',
    components: {SolarButton}
})
export default class Orders extends Vue {}
</script>
<style scoped lang="scss">

</style>