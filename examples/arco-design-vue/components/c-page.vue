<template>
    <a-config-provider :locale="locale.getCurrentComponent()">
        <slot></slot>
    </a-config-provider>
</template>
<script>
export default {
    props: ["api"],
    setup(props) {
        const locale = inject("locale");
        onMounted(async () => {
            var response = await fetch("/api/dotnet/api/site/locale");
            var result = await response.json();
            console.log(result);
        });
        //theme
        const theme = reactive({
            current: GetOrAddLocalStorageItem("theme", "light"),
            items: ["light", "dark"],
        });
        theme.change = (o) => {
            theme.current = UpdateLocalStorageItem("theme", o);
            if (theme.current === "dark") {
                document.body.setAttribute("arco-theme", "dark");
            } else {
                document.body.removeAttribute("arco-theme");
            }
        };
        theme.change(theme.current);
        provide("theme", theme);
        //webapi
        const webapi = reactive({
            current: GetOrAddLocalStorageItem("webapi", "mock"),
            items: ["mock", "dotnet", "java"],
        });
        webapi.content = (o) => {
            return `api/${webapi.current}/${o}`;
        };
        provide("webapi", webapi);
        //site
        const site = reactive({
            title: "Html Title",
            name: "SiteName",
            logo: "logo.svg",
            copyright: "copyright",
            version: "0.1.0",
        });
        watch(
            () => site.title,
            (value, oldValue) => {
                document.title = value;
            }
        );
        provide("site", site);
        const user = reactive({
            username: null,
        });
        provide("user", user);
        //test start
        //var url =
        //test end
        //return
        return {
            locale,
        };
    },
};
</script>
<style>
body,
body > div {
    margin: 0;
    padding: 0;
    height: 100vh;
    color: var(--color-text-1);
    background-color: var(--color-bg-1);
    overflow: hidden;
}

a {
    color: var(--color-text-1);
    background-color: var(--color-bg-1);
    text-decoration: none;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.c-container {
    box-sizing: border-box;
    height: 100vh;
    padding-top: 60px;
}

.c-body {
    height: 100%;
}

.arco-layout-header,
.arco-layout-footer {
    box-sizing: border-box;
    display: flex;
    background-color: var(--color-bg-1);
}

.arco-layout-header {
    position: fixed;
    top: 0;
    right: 0;
    width: 100vw;
    border-bottom: 1px solid var(--color-border);
    z-index: 100;
    overflow: hidden;
    justify-content: space-between;
    height: 60px;
    padding: 0 12px;
}
</style>
