<template>
  <a-layout class="c-container">
    <a-layout-header>
      <c-header v-model="site"></c-header>
    </a-layout-header>
    <a-layout class="c-body">
      <a-layout-sider
        collapsible
        breakpoint="xl"
        class="c-aside"
        :style="{ width: 'auto' }"
      >
        <a-menu
          accordion
          :selected-keys="[menu.current]"
          @menu-item-click="onMenuItemClick"
          :style="{ width: '100%' }"
        >
          <c-menu v-model="menu.items"></c-menu>
        </a-menu>
      </a-layout-sider>
      <a-layout class="c-content">
        <a-layout-content>
          <slot></slot>
        </a-layout-content>
        <a-layout-footer>
          <c-footer v-model="site"></c-footer>
        </a-layout-footer>
      </a-layout>
      <a-back-top target-container=".c-content" :style="{ position: 'absolute' }">
        <a-button shape="circle">
          <c-icon name="icon-up"></c-icon>
        </a-button>
      </a-back-top>
    </a-layout>
  </a-layout>
</template>
<script>
export default {
  setup() {
    //site
    const site = inject("site");
    //menu
    const route = useRoute();
    const menu = reactive({
      current: route.path,
      items: [],
    });
    const webapi = inject("webapi");
    const init = () => {
      fetch(webapi.content("menu.json"))
        .then((o) => o.json())
        .then((o) => (menu.items = o));
    };
    //mounted
    onMounted(init);
    //return
    return {
      site,
      menu,
    };
  },
};
</script>
<style>
.arco-layout-sider {
  position: relative;
}

.arco-layout-sider-children {
  height: 100%;
  overflow: auto;
}

.arco-layout-sider-has-trigger {
  box-sizing: border-box;
  padding-bottom: 48px;
}

.arco-layout-sider-trigger {
  overflow-y: hidden;
}

.arco-menu-selected + .arco-menu-inline-content {
  display: block !important;
}
</style>
