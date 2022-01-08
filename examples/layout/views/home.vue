<template>
    <layout>
        <h1>Home</h1>
    </layout>
</template>
<script>
export default {
    setup() {
        console.log(inject("site"));
    },
};
</script>
