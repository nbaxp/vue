<template>
    <c-layout>
        <p>
            # table 1.根据 url query 初始化查询的 json对象
            2.从服务端获取查询结果，包括 schema 对象、查询表单的 json
            对象、返回的查询结果、当前用户的权限 3.根据 schema 和 json
            生成查询表单，更新 url query 4.根据查询结果生成 table 和 分页
            5.根据schema生成table的列标题，格式化table的单元格数据
            6.根据权限，选择是否生成：详情、编辑和删除
        </p>
        <c-table v-model="model.form"></c-table>
        <hr>
        <p>current:{{model.form.current}}|{{model.form.pageSize}}|{{model.form.total}}</p><button @click="model.form={current:model.form.current+1,total:100,pageSize:30}">测试</button>
    </c-layout>
</template>
<script>
const reactive  = Vue.reactive;
const onMounted = Vue.onMounted;

export default {
    setup() {
        const model = reactive ({
            //paged
            //data
            //schema
            //form
        });
        model.form=Qs.parse(location.search?location.search.substr(1):null);
        const init = () => {
            var query = Qs.stringify(model.form);
            var url = `/api/table.json${query?('?'+query):''}`;
            fetch(url)
                .then((o) => o.json())
                .then((o) => {
                    model.form = o;
                });
        };
        const change = (e) => {
            init();
            console.dir(e);
        };
        onMounted(init);
        return {
            model,
            init,
            change,
        };
    },
};
</script>
