<template>
    <slot></slot>
</template>
<script>
export default {
    props: ["layout"],
    setup(props) {
        console.log(props.layout);
        const site = inject("site");
        if (props.layout && site.layout !== props.layout) {
            site.layout = props.layout;
        }
    },
};
</script>
