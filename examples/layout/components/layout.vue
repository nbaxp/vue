<template>
    <component :is="site.layout">
        <slot></slot>
    </component>
</template>
<script>
export default {
    setup() {
        const defaultLayout = "layout-default";
        const site = reactive({
            defaultLayout: defaultLayout,
            layout: defaultLayout,
        });
        provide("site", site);
        return {
            site,
        };
    },
};
</script>
