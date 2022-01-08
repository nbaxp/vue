<template>
    <router-link to="/">Home</router-link>
    <router-link to="/login">Login</router-link>
    <h1>Layout Default</h1>
    <slot> </slot>
</template>
<script>
export default {
    setup() {
        console.log(inject("site"));
    },
};
</script>