<template>
    <component :is="currentLayout">
        <slot></slot>
    </component>
</template>
<script>
export default {
    props: ["layout"],
    setup(props) {
        //theme
        const theme = reactive({
            current: GetOrAddLocalStorageItem("theme", "light"),
            items: ["light", "dark"],
        });
        provide("theme", theme);
        theme.change = (o) => {
            theme.current = UpdateLocalStorageItem("theme", o);
            if (theme.current === "dark") {
                document.body.setAttribute("arco-theme", "dark");
            } else {
                document.body.removeAttribute("arco-theme");
            }
        };
        theme.change(theme.current);
        //layout
        let currentLayout = props.layout ?? "c-layout-default";
        return {
            currentLayout,
        };
    },
};
</script>
