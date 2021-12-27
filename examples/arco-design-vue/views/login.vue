<template>
    <c-layout layout="c-layout-user">
        <h2>{{ $t("message.home") }}</h2>
        <c-form :schema="schema"></c-form>
        <a-button @click="login">Submit</a-button>
    </c-layout>
</template>
<script>
export default {
    setup() {
        const webapi = inject("webapi");
        const token = inject("token");
        const router = useRouter();
        inject("site").title = "登录";
        const model = reactive({
            username: null,
            password: null,
        });
        const schema = {
            title: "登录",
            type: "object",
            properties: {
                username: {
                    title: "用户名",
                    type: "string",
                    required: "必须填写{0}",
                },
                password: {
                    title: "用户名",
                    type: "string",
                    required: "必须填写{0}",
                },
            },
        };
        const errors = ref([]);
        return {
            title: "登录",
            model,
            schema,
            errors,
            login() {
                const url = `${location.protocol}//${location.host}/api/dotnet/token`;
                fetch(url, {
                    method: "POST",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded",
                        "Cache-Control": "no-cache",
                    },
                    body: "username=admin",
                })
                    .then((o) => o.json())
                    .then((o) => {
                        token.access_token = o.access_token;
                        token.expiry = o.expires_in;
                        console.debug("登录成功，开始刷新token");
                        token.startRefresh();
                        router.push("/");
                    });
            },
        };
    },
};
</script>
