<template>
<div>
    <h2>分页：:{{modelValue.current}}|{{modelValue.pageSize}}</h2>
    <a-pagination
        show-total
        :total="modelValue.total"
        :current="modelValue.current"
        :page-size="modelValue.pageSize"
        @change="change"
        @page-size-change="pageSizeChange"
    />
</div>
</template>
<script>
export default {
    props: ["modelValue"],
    emits:["update:modelValue"],
    setup(props){
        return {
            change(event){
                props.modelValue.current = event;
            },
            pageSizeChange(event){
                props.modelValue.pageSize = event;
            }
        }
    }
};
</script>
