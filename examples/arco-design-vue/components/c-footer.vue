<template>
    <a-space>
        <div v-html="modelValue.copyright"></div>
    </a-space>
</template>
<script>
export default {
    props: ["modelValue"],
};
</script>
<style>
.arco-layout-footer {
    justify-content: space-around;
    border-top: 1px solid var(--color-border);
    padding: 12px;
}
</style>
