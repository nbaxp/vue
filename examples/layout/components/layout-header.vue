<template>
    <router-link to="/">Home</router-link>
    <router-link to="/about">About</router-link>
    <router-link to="/login">Login</router-link>
</template>
<script>
export default {
    setup() {
        console.log(`layout: ${inject("site").layout} header init`);
    },
};
</script>