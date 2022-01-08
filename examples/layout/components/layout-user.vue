<template>
    <layout-header></layout-header>
    <h1>Layout User</h1>
    <slot> </slot>
</template>
<script>
export default {
    setup() {
        console.log(inject("site"));
    },
};
</script>
