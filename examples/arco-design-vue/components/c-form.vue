<template>
    <h2>{{ schema.title }}</h2>
    <template v-for="item in schema.properties">
        <div>
            <template v-if="item.oneOf">
                <label>${{ item.title }}=>{{ item.oneOf }}</label>
            </template>
            <template v-else>
                <label>#{{ item.title }}:</label>
            </template>
        </div>
    </template>
</template>
<script>
export default {
    props: ["schema"],
    setup(props) {
        const items = ref([]);
        return {
            items,
        };
    },
};
</script>
