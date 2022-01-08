window.onMounted = Vue.onMounted;
window.onUnmounted = Vue.onUnmounted;
window.ref = Vue.ref;
window.reactive = Vue.reactive;
window.watch = Vue.watch;
window.provide = Vue.provide;
window.inject = Vue.inject;
window.useRouter = VueRouter.useRouter;
window.useRoute = VueRouter.useRoute;
const createApp = Vue.createApp;
const defineAsyncComponent = Vue.defineAsyncComponent;
const createRouter = VueRouter.createRouter;
const createWebHistory = VueRouter.createWebHistory;
const app = createApp({
    setup() {
        return {};
    }
});
const router = new createRouter({
    history: createWebHistory(
        document.querySelector("base")?.getAttribute("href")
    ),
    routes: [
        // {
        //     path: "/:catchAll(.*)",
        //     redirect: "/404.html"
        // }
    ],
});
app.use(router);
VueBrowserSfc.config.debug = true;
VueBrowserSfc.config.componentExt = ".vue";
app.use(VueBrowserSfc, defineAsyncComponent);
app.mount("#root");