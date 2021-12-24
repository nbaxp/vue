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
        <c-table v-model="model.form" @load="nav"></c-table>
    </c-layout>
</template>
<script>
export default {
    setup() {
        const webapi = inject("webapi");
        const router = useRouter();
        const route = useRoute();
        const model = reactive({
            //paged
            //data
            //schema
            //form
        });
        model.schema = {
            title: "列表项",
            type: "object",
            properties: {
                count: {
                    title: "总数",
                    type: "integer",
                },
                pageSize: {
                    title: "页码",
                    type: "integer",
                },
                pageIndex: {
                    title: "当前页",
                    type: "integer",
                },
                name: {
                    title: "string类型",
                    type: "string",
                },
            },
        };
        model.form = Qs.parse(
            location.search ? location.search.substr(1) : null
        );
        const init = () => {
            var query = Qs.stringify(model.form);
            var url = webapi.content(`table.json${query ? "?" + query : ""}`);
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
            nav() {
                let params = new URLSearchParams(Qs.stringify(model.form));
                params.delete("total");
                if (params.get("pageIndex") == 1) {
                    params.delete("pageIndex");
                }
                if (params.get("pageSize") == 10) {
                    params.delete("pageSize");
                }
                var query = params.toString();
                var path = route.path + (query ? "?" + query : "");
                router.push(path);
            },
        };
    },
};
</script>
