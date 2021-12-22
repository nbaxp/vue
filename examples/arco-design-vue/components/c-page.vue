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
      version: "0.1.0",
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

.arco-layout-footer {
  justify-content: space-between;
  border-top: 1px solid var(--color-border);
  padding: 12px;
}
</style>
