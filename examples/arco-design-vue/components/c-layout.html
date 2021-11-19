<template>
  <a-config-provider :locale="locale.current">
    <a-layout class="c-container">
      <a-layout-header>
        <a-space>
          <router-link to="/" class="c-logo">
            <a-space>
              <img :alt="name" :src="site.logo">
              <h1>{{site.name}}</h1>
            </a-space>
          </router-link>
        </a-space>
        <a-space>
          <a-button shape="circle" @click="changeTheme">
            <c-icon :name="site.dark?'icon-moon':'icon-sun'"></c-icon>
          </a-button>
          <a-dropdown trigger="hover" @select="changeLocale">
            <a-button>{{getLocaleName(locale.current.locale)}}</a-button>
            <template #content>
              <a-doption v-for="item in locale.items">{{getLocaleName(item.locale)}}</a-doption>
            </template>
          </a-dropdown>
          <a-dropdown v-if="user" trigger="hover">
            <a-button>
              <c-icon :name="user.icon"></c-icon> {{user.name}}
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
        <a-layout-sider collapsible breakpoint="xl" class="c-aside" :style="{width:'auto'}">
          <a-menu accordion :selected-keys="[menu.current]" @menu-item-click="onMenuItemClick" :style="{ width: '100%' }">
            <c-menu v-model="menu.items"></c-menu>
          </a-menu>
        </a-layout-sider>
        <a-layout class="c-r">
          <a-layout-content>
            <slot></slot>
          </a-layout-content>
          <a-layout-footer>
            <a-space v-for="item in site.copyright">
              <div v-html="item"></div>
            </a-space>
          </a-layout-footer>
        </a-layout>
        <a-back-top target-container=".c-r" :style="{position:'absolute'}">
          <a-button shape="circle">
            <c-icon name="icon-up"></c-icon>
          </a-button>
        </a-back-top>
      </a-layout>
    </a-layout>
  </a-config-provider>
</template>
<script>
  import zhCN from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/zh-cn.js";
  import enUS from "https://cdn.jsdelivr.net/npm/@arco-design/web-vue@2.3.0/es/locale/lang/en-us.js";
  const ref = Vue.ref;
  const onMounted = Vue.onMounted;
  const useRouter = VueRouter.useRouter;
  const useRoute = VueRouter.useRoute;

  export default {
    props:['title'],
    setup(props) {
      document.title = props.title;
      const router = useRouter();
      const route = useRoute();
      const site = ref({
        name: "Arco Design Vue",
        logo: 'logo.svg',
        copyright: [
          `© ${new Date().getFullYear()} Company. All rights reserved`,
          `<a target="blank" href="https://v3.cn.vuejs.org/">Vue 3</a> | <a target="blank" href="https://arco.design/vue/docs/start">Arco Design Vue</a>`
        ],
        dark: false
      });

      const locale=ref({
        current: zhCN,
          items: [zhCN, enUS]
      });
      const user=ref({
          name: "admin",
          icon: "icon-user"
      });
      const menu=ref({
        current: route.path,
          items: []
      });
      const init = () => {
        fetch("api/menu.json")
          .then((o) => o.json())
          .then((o) => menu.value.items = o);
      };
      const getLocaleByName=name=>name === "简体中文" ? zhCN : enUS;
      onMounted(init);
      return {
        site,
        locale,
        menu,
        user,
        changeLocale(name) {
          locale.value.current = getLocaleByName(name);
        },
        getLocaleName(name) {
          return name == 'zh-CN' ? '简体中文' : 'English';
        },
        getLocaleByName,
        changeTheme() {
          site.value.dark = !site.value.dark;
          if (site.value.dark) {
            document.body.setAttribute('arco-theme', 'dark');
          }
          else {
            document.body.removeAttribute('arco-theme');
          }
        },
        onMenuItemClick(key) {
          router.push(key);
        }
      }
    }
  };
</script>
<style>
  body,
  body>div {
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

  .arco-menu-selected+.arco-menu-inline-content {
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