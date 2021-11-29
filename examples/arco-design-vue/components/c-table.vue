<template>
    <a-pagination
        show-total
        hide-on-single-page
        :total="modelValue.total"
        :current="modelValue.current"
        @change="modelValue.current=$event"
        @pageSizeChange="modelValue.pageSize=$event"
    />
</template>
<script>
export default {
    props: ["modelValue"],
    emits:["update:modelValue"],
    setup(){
        return {
        }
    }
};
</script>
