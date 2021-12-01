<template>
    <div :style="style" ref="icontainer" class="iframe-container">
        <iframe ref="iframe" frameborder="0"></iframe>
    </div>
</template>
<script>
    const ref = Vue.ref;
    const onMounted = Vue.onMounted;

    export default {
        props: ["src"],
        setup(props){
            const iframe = ref(null);
            onMounted(() =>iframe.value.src = props.src);
            return {
                iframe
            };
        }
    };
</script>
<style>
    .iframe-container>iframe {
        width: 100%;
        height: 100%;
    }
</style>