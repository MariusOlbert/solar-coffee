<template>
  <div id="app">
    <div class="app-menu">
      <side-menu />
    </div>
    <div class="app-content">
      <router-view />
    </div>    
  </div>
</template>

<style lang="scss">
@import '@/scss/global.scss';
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  display: flex;
  flex-direction: row;

  .app-menu {
    position: fixed;
  }
  .app-content {
    padding: 1.2rem;
    width: 90%;
    margin: 1rem auto 1rem $menu-width;
    text-align: left;
  }
}
* {
  margin: 0;
}
a {
  text-decoration: none;
}
hr {
  margin: 1rem 0;
  border: 1px solid #eee;
}
</style>
<script lang="ts">
 import {Component, Vue} from 'vue-property-decorator';
 import SideMenu from '@/components/SideMenu.vue';
 @Component({
   name: 'App',
   components: {SideMenu}
 })
 export default class App extends Vue {}
</script>
