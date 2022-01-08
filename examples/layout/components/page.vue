<template>
    <slot></slot>
</template>
<script>
export default {
    props: ["layout"],
    setup(props) {
        const site = inject("site");
        if (props.layout) {
            if (site.layout !== props.layout) {
                site.layout = props.layout;
            }
        } else {
            if (site.layout !== site.defaultLayout) {
                site.layout = site.defaultLayout;
            }
        }
         console.log(`layout: ${inject("site").layout}`);
    },
};
</script>
