<template>
    <component :is="name" />
</template>
<script>
export default {
    props: ["name"],
};
</script>
