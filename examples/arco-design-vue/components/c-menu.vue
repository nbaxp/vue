<template>
    <template v-for="item in modelValue">
        <a-menu-item-group
            v-if="item.type && item.type === 'group'"
            :title="item.name"
        >
            <c-menu v-model="item.children"></c-menu>
        </a-menu-item-group>
        <a-sub-menu
            v-else-if="item.children && item.children.length"
            :key="item.path"
        >
            <template #title
                ><c-icon :name="item.icon ?? menuIcon"></c-icon>
                {{ item.name }}</template
            >
            <c-menu v-model="item.children"></c-menu>
        </a-sub-menu>
        <a-menu-item v-else :key="item.path">
            <c-icon :name="item.icon ?? itemIcon"></c-icon>
            {{ item.name }}
        </a-menu-item>
    </template>
</template>
<script>
export default {
    props: ["modelValue"],
    setup() {
        const menuIcon = ref("icon-folder");
        const itemIcon = ref("icon-file");
        return {
            menuIcon,
            itemIcon,
        };
    },
};
</script>
