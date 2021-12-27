<template>
    <a-layout class="c-container">
        <a-layout-header class="c-header">
            <c-header v-model="site"></c-header>
        </a-layout-header>
        <a-layout-content>
            <slot></slot>
        </a-layout-content>
        <a-layout-footer>
            <c-footer v-model="site"></c-footer>
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
