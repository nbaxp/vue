<template>
    <a-config-provider :locale="locale.items.get(locale.current)">
        <a-layout class="c-container">
            <a-layout-header>
                <a-space>
                    <router-link to="/" class="c-logo">
                        <a-space>
                            <img :alt="site.name" :src="site.logo" />
                            <h1>{{ site.name }}</h1>
                        </a-space>
                    </router-link>
                </a-space>
                <a-space>
                        <a-select :default-value="webapi.current" @change="changeBe">
                            <a-option v-for="item in webapi.items" :key="item">{{item}}</a-option>
                        </a-select>
                    <a-button shape="circle" @click="theme.change(theme.current=='light'?'dark':'light')">
                        <c-icon :name="theme.current==='dark' ? 'icon-moon' : 'icon-sun'"></c-icon>
                    </a-button>
                    <a-dropdown trigger="hover" @select="locale.change">
                        <a-button>{{locale.current}}</a-button>
                        <template #content>
                            <a-doption v-for="item in locale.items" :key="item[0]">{{item[0]}}</a-doption>
                        </template>
                    </a-dropdown>
                    <a-dropdown v-if="user" trigger="hover">
                        <a-button>
                            <c-icon :name="user.icon"></c-icon> {{ user.name }}
                        </a-button>
                        <template #content>
                            <a-doption>
                                <router-link to="/user">个人中心</router-link>
                            </a-doption>
                            <a-doption>
                                <router-link to="/logout">退出</router-link>
                            </a-doption>
                        </template>
                    </a-dropdown>
                    <router-link v-else to="/login">登录</router-link>
                </a-space>
            </a-layout-header>
            <a-layout class="c-body">
                <a-layout-sider
                    collapsible
                    breakpoint="xl"
                    class="c-aside"
                    :style="{ width: 'auto' }"
                >
                    <a-menu
                        accordion
                        :selected-keys="[menu.current]"
                        @menu-item-click="onMenuItemClick"
                        :style="{ width: '100%' }"
                    >
                        <c-menu v-model="menu.items"></c-menu>
                    </a-menu>
                </a-layout-sider>
                <a-layout class="c-r">
                    <a-layout-content>
                        <slot></slot>
                    </a-layout-content>
                    <a-layout-footer>
                        <a-space v-for="item in site.copyright" :key="item">
                            <div v-html="item"></div>
                        </a-space>
                    </a-layout-footer>
                </a-layout>
                <a-back-top
                    target-container=".c-r"
                    :style="{ position: 'absolute' }"
                >
                    <a-button shape="circle">
                        <c-icon name="icon-up"></c-icon>
                    </a-button>
                </a-back-top>
            </a-layout>
        </a-layout>
    </a-config-provider>
</template>
<script>
const reactive = Vue.reactive;
const inject = Vue.inject;
const onMounted = Vue.onMounted;
const useRouter = VueRouter.useRouter;
const useRoute = VueRouter.useRoute;

export default {
    props: ["title"],
    setup(props) {
        document.title = props.title;
        const router = useRouter();
        const route = useRoute();
        const webapi=inject('webapi');
        const theme=inject('theme');
        const locale=inject('locale');
        const site = reactive({
            name: "Arco Design Vue",
            logo: "logo.svg",
            copyright: [
                `© ${new Date().getFullYear()} Company. All rights reserved`,
                `<a target="blank" href="https://v3.cn.vuejs.org/">Vue 3</a> | <a target="blank" href="https://arco.design/vue/docs/start">Arco Design Vue</a>`,
            ],
            dark: false,
        });

        const user = reactive({
            name: "admin",
            icon: "icon-user",
        });
        const menu = reactive({
            current: route.path,
            items: [],
        });
        const init = () => {
            fetch(webapi.content('menu.json'))
                .then((o) => o.json())
                .then((o) => (menu.items = o));
        };
        onMounted(init);
        return {
            webapi,
            theme,
            locale,
            site,
            menu,
            user,
            onMenuItemClick(key) {
                router.push(key);
            },
            changeBe(e){
                webapi.current=LocalStorageUpdateItem('webapi',e);
                location.reload();
            }
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

.arco-layout-sider {
    position: relative;
}

.arco-layout-sider-children {
    height: 100%;
    overflow: auto;
}

.arco-layout-sider-has-trigger {
    box-sizing: border-box;
    padding-bottom: 48px;
}

.arco-layout-sider-trigger {
    overflow-y: hidden;
}

.arco-menu-selected + .arco-menu-inline-content {
    display: block !important;
}
</style>
<style>
.c-logo {
    display: inline-flex;
    align-items: center;
}

.c-logo img {
    height: 24px;
}

.c-logo h1 {
    line-height: 1.4;
    font-size: 18px;
    color: var(--color-text-1);
}
</style>
