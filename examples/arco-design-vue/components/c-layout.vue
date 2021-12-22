<template>
    <component :is="currentLayout">
        <slot></slot>
    </component>
</template>
<script>
export default {
    props: ["layout"],
    setup(props){
        let currentLayout = props.layout?? "c-layout-default";
        return {
            currentLayout
        };
    }
};
</script>
