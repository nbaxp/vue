import { createApp, defineAsyncComponent, ref, reactive, provide, inject, onMounted, onUnmounted } from "vue";
import { createRouter, createWebHashHistory } from "vue-router";
import * as VueBrowserSfc from "vue-browser-sfc";
window.ref = ref;
window.reactive = reactive;
window.provide = provide;
window.inject = inject;
const app = createApp({
    setup() {
        return {};
    }
});

const router = new createRouter({
    history: createWebHashHistory(
        document.querySelector("base")?.getAttribute("href")
    ),
    routes: [
    ],
});

app.use(router);

VueBrowserSfc.config.debug = true;
VueBrowserSfc.config.componentExt = ".vue";
app.use(VueBrowserSfc, defineAsyncComponent);

app.mount("#root");