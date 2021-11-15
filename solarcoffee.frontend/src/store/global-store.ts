import {make } from 'vuex-pathify';
import { IInventoryTimeline } from "@/types/InventoryGraph";
import InventoryService from '@/services/InventoryService';

class GlobalStore {
    snapshotTimeline: IInventoryTimeline = {
        productInventorySnapshots: [],
        timeline: []
    };
    isTimelineBuilt: boolean = false;
}

const state = new GlobalStore();
const mutations = make.mutations(state);
const actions = {
    async assignSnapshots({commit}: {commit: any}) {
        const inventoryService = new InventoryService();
        let res = await inventoryService.getSnapshotHistory();
        console.log(":: ReS ::", res.timeLine)
        let timeline: IInventoryTimeline = {
            productInventorySnapshots: res.productInventorySnapshots,
            timeline: res.timeLine
        };
        console.log("timeline", timeline)
        commit('SET_SNAPSHOT_TIMELINE', timeline);
        commit('SET_IS_TIMELINE_BUILT', true);
    },
}
const getters = {};

export default {
    state,
    mutations,
    actions,
    getters
};