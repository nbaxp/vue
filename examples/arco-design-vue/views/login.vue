<template>
    <a-form :model="form" :style="{ width: '600px' }">
        <a-form-item>
            <a-button @click="login">Submit</a-button>
        </a-form-item>
    </a-form>
</template>
<script>
export default {
    setup() {
        const webapi = inject("webapi");
        const token = inject("token");
        return {
            login() {
                const url = `${location.protocol}//${location.host}/api/dotnet/token?username=admin`;
                fetch(url, {
                    method: "POST",
                    credentials: "include",
                    headers: {
                        //"Content-Type": "application/json",
                        "Cache-Control": "no-cache",
                    },
                    body: '"admin"',
                })
                    .then((o) => o.json())
                    .then((o) => {
                        token.access_token = o.access_token;
                        token.expiry = o.expires_in;
                        console.debug("登录成功，开始刷新token");
                        token.startRefresh();
                        
                    });
            },
        };
    },
};
</script>
