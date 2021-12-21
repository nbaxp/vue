<template>
    <form>
        <fieldset>
            <legend>{{ schema.title }}</legend>
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
        </fieldset>
    </form>
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
