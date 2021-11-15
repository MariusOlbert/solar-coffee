import {shallowMount} from "@vue/test-utils";
import SolarButton from "@/components/SolarButton.vue";

describe('SolarButton.vue', ()=> {
    it("display text in default slot position", ()=>{
        const wrapper = shallowMount(SolarButton, {
            propsData: {
                link: false
            },
            slots: {
                default: 'click here!'
            }
        });
        expect(wrapper.find('button').text()).toBe('click here!')
    });
    it("has underlying disabled button when disabled passed as prop", ()=>{
        const wrapper = shallowMount(SolarButton, {
            propsData: {
                disabled: true
            },
            slots: {
                default: 'foo'
            }
        });
        expect(wrapper.find('input:disabled'))
    });
    it("has not has underlying disabled button when disabled passed as prop", ()=>{
        const wrapper = shallowMount(SolarButton, {
            propsData: {
                disabled: true
            },
            slots: {
                default: 'foo'
            }
        });
        expect(!wrapper.find('input:disabled'))
    });
})