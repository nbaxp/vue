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
