<template>
    <a-layout class="c-container">
        <a-layout-header>Layout Default</a-layout-header>
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
            <a-layout-content>
                <slot></slot>
            </a-layout-content>
        </a-layout>
        <a-layout-footer>
            <c-copyright v-model="site.copyright"></c-copyright>
        </a-layout-footer>
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
