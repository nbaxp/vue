<template>
    <a-layout>
        <a-layout-header>Layout User</a-layout-header>
        <a-layout-content>
            <slot></slot>
        </a-layout-content>
        <a-layout-footer>
            <c-copyright v-model="site.copyright"></c-copyright>
        </a-layout-footer>
    </a-layout>
</template>
<script>
export default {
    setup() {
        const site = inject("site");
        return {
            site,
        };
    },
};
</script>
