import zhCN from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/zh-cn.js";
import enUS from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/en-us.js";
const createApp = Vue.createApp;
const createRouter = VueRouter.createRouter;
const createWebHistory = VueRouter.createWebHistory;
const createWebHashHistory = VueRouter.createWebHashHistory;
const defineAsyncComponent = Vue.defineAsyncComponent;
window.onMounted = Vue.onMounted;
window.onUnmounted = Vue.onUnmounted;
window.ref = Vue.ref;
window.reactive = Vue.reactive;
window.watch = Vue.watch;
window.provide = Vue.provide;
window.inject = Vue.inject;
window.useRouter = VueRouter.useRouter;
window.useRoute = VueRouter.useRoute;
window.createI18n = VueI18n.createI18n;

///webapi
const webapi = reactive({
    current: localStorage.getItem("webapi") ?? "dotnet",
    items: ["dotnet", "java"],
});
webapi.change = o => {
    webapi.current = o;
    localStorage.setItem("webapi", o);
};
webapi.content = (o) => {
    return `${location.protocol}//${location.host}/api/${webapi.current}/${o}`
};

///locale
//加载服务端locales
const locales = await (await fetch(webapi.content("site/locale"))).json();
//获取本地缓存的locale
var currentLocale = localStorage.getItem("locale");
if (!currentLocale) {//不存在缓存，设置当前locale为缓存
    currentLocale = locales.current;
    localStorage.setItem("locale", currentLocale);
}
else {//存在缓存
    if (locales.items.find(o => o.name === currentLocale)) {//缓存的locale存在，更改当前的locale为缓存的locale
        locales.current = currentLocale;
    }
    else {//缓存的locale不存在，设置当前locale为缓存
        localStorage.setItem("locale", locales.current);
    }
}
//https://vue-i18n.intlify.dev/guide/essentials/scope.html#local-scope-1
const i18n = createI18n({
    locale: locales.current,
    fallbackLocale: "zh-CN",
    messages: {
        "zh-CN": {
            message: {
                theme: {
                    light: "亮色模式",
                    dark: "暗黑模式",
                    system: "跟随系统"
                },
                home: "首页"
            }
        },
        "en-US": {
            message: {
                theme: {
                    light: "Light Mode",
                    dark: "Dar Mode",
                    system: "System theme"
                },
                home: "home"
            }
        }
    }
});
const locale = reactive({
    current: locales.current,
    items: locales.items,
    components: new Map([["zh-CN", zhCN], ["en-US", enUS]]),
    i18n,
});

locale.getCurrentComponent = () => locale.components.get(locale.current);
locale.getNativeName = o => locale.items.find(l => l.name === o).nativeName;
locale.change = o => {
    locale.current = o;
    localStorage.setItem("locale", locale.current);
    locale.i18n.global.locale = locale.current;
};
///app
const app = createApp({
    setup(props, context) {
        //webapi
        provide("webapi", webapi);
        //locale
        provide("locale", locale);
        //router
        const router = useRouter();
        //token
        const token = reactive({
            access_token: null,
            expiry: null
        });
        const logoutKey = 'logout';
        token.logout = () => {
            const url = webapi.content('logout');
            fetch(url, {
                method: 'POST',
                credentials: 'include',
            }).then(o => {
                window.localStorage.setItem(logoutKey, Date.now());
            });
        };
        token.refresh = async () => {
            const url = webapi.content('refresh_token');
            try {
                const response = await fetch(url, {
                    method: "POST",
                    credentials: "include",
                });
                if (response.status === 200) {
                    const result = await response.json();
                    token.access_token = result.access_token;
                    token.expiry = result.expires_in;
                    return true;
                }
                else {
                    console.log(response.statusText);
                }
            } catch (error) {
                console.log(error);
            }
            return false;
        };
        token.startRefresh = () => {
            token.timer = setInterval(async () => {
                const exp = jwt_decode(token.access_token).exp * 1000;
                if (exp - Date.now() < 1000 * 10) {
                    console.debug(`已过期，开始刷新token`);
                    if (!await token.refresh()) {
                        console.debug('刷新失败，停止定时刷新，跳转到登录页');
                        clearInterval(token.timer);
                        token.access_token = null;
                        router.push('/login');
                    }
                    else {
                        console.debug('刷新token成功');
                    }
                }
                else {
                    console.debug(`无需刷新token,到期时间：${new Date(exp)}`);
                }
            }, 1000 * 200);
        };
        const logout = (e) => {
            if (e.key === logoutKey) {
                clearInterval(token.timer);
                token.access_token = null;
                router.push('/login');
            }
        };
        provide('token', token);
        onMounted(async () => {
            //const locale = fetch()
            window.addEventListener('storage', logout);
            //token
            console.debug('会话开始，尝试刷新token');
            if (await token.refresh()) {
                console.debug('刷新成功，开始定时刷新token');
                token.startRefresh();
            }
            else {
                console.debug('刷新失败，跳转到登录页');
                router.push('/login');
            }
        });
        //onUnmounted
        onUnmounted(() => {
            window.removeEventListener('storage', logout);
            window.localStorage.removeItem('logout');
        });
    }
});
window.app = app;
app.config.compilerOptions.isCustomElement = tag => tag.startsWith('ion-');

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
app.use(i18n);
app.use(router);
app.use(ArcoVue);
app.use(ArcoVueIcon);

VueBrowserSfc.config.debug = true;
VueBrowserSfc.config.componentExt = ".vue";
app.use(VueBrowserSfc, defineAsyncComponent);
app.mount("#root");

///////////////////////////////////////////////////////////////
const connection = new signalR.HubConnectionBuilder()
    .withUrl(webapi.content("hub"))
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();