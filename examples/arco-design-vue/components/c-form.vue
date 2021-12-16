<template>
    <a-form :model="modelValue">
        <a-form-item
            v-for="item in items"
            :field="item.field"
            :label="item.label"
        >
            <a-input v-model="item.name" :placeholder="item.description" />
            <template #help>
                <div>help message</div>
            </template>
        </a-form-item>
    </a-form>
</template>
<script>
export default {
    props: ["modelValue"],
    setup(props) {
        const items = ref([]);
        return {
            items,
        };
    },
};
</script>
