<template>
    <a-config-provider :locale="locale.items.get(locale.current)">
        <slot></slot>
    </a-config-provider>
</template>
<script>
import zhCN from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/zh-cn.js";
import enUS from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/en-us.js";

export default {
    setup() {
        //locale
        const locale = reactive({
            current: GetOrAddLocalStorageItem("locale", "简体中文"),
            items: new Map([
                ["简体中文", zhCN],
                ["English", enUS],
            ]),
        });
        locale.change = (o) => {
            locale.current = UpdateLocalStorageItem("locale", o);
        };
        provide("locale", locale);
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
        });
        watch(
            () => site.title,
            (value, oldValue) => {
                document.title = value;
            }
        );
        provide("site", site);
        //return
        return {
            locale,
        };
    },
};
</script>
