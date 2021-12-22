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

const app = createApp({
    setup() {
        const router = useRouter();
        

        //theme
        const theme = reactive({
            current: GetOrAddLocalStorageItem('theme', 'light'),
            items: ['light', 'dark']
        });
        provide('theme', theme);
        theme.change = o => {
            theme.current = UpdateLocalStorageItem('theme', o);
            if (theme.current === 'dark') {
                document.body.setAttribute("arco-theme", "dark");
            } else {
                document.body.removeAttribute("arco-theme");
            }
        };
        theme.change(theme.current);

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
            const url = `${location.protocol}//${location.host}/api/dotnet/refresh_token`;
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
            window.addEventListener('storage', logout);
            //load
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
        //return
        return {};
    }
});
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
app.use(router);

app.use(ArcoVue);
app.use(ArcoVueIcon);

VueBrowserSfc.config.debug = true;
VueBrowserSfc.config.componentExt = ".vue";
app.use(VueBrowserSfc, defineAsyncComponent);
app.mount("#root");

///////////////////////////////////////////////////////////////
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/api/dotnet/hub")
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