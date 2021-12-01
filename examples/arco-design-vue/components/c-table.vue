<template>
<div>
    <a-pagination
        show-total
        show-page-size
        :total="modelValue.total"
        :current="modelValue.pageIndex"
        :page-size="modelValue.pageSize"
        @change="change"
        @page-size-change="pageSizeChange"
    />
</div>
</template>
<script>
export default {
    props: ["modelValue"],
    emits:["update:modelValue","load"],
    setup(props,context){
        return {
            change(event){
                props.modelValue.pageIndex = event;
                context.emit('load');
            },
            pageSizeChange(event){
                props.modelValue.pageIndex = 1;
                props.modelValue.pageSize = event;
                context.emit('load');
            }
        }
    }
};
</script>
