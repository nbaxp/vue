<template>
    <a-space>
        <router-link to="/" class="c-logo">
            <a-space>
                <img :alt="modelValue.name" :src="modelValue.logo" />
                <h1>{{ modelValue.name }}</h1>
            </a-space>
        </router-link>
    </a-space>
    <a-space>
        <a-select :default-value="webapi.current" @change="changeBe">
            <a-option v-for="item in webapi.items" :key="item">{{
                item
            }}</a-option>
        </a-select>
        <a-button
            shape="circle"
            @click="theme.change(theme.current == 'light' ? 'dark' : 'light')"
        >
            <c-icon
                :name="theme.current === 'dark' ? 'icon-moon' : 'icon-sun'"
            ></c-icon>
        </a-button>
        <a-dropdown trigger="hover" @select="locale.change">
            <a-button>{{ locale.current }}</a-button>
            <template #content>
                <a-doption v-for="item in locale.items" :key="item[0]">{{
                    item[0]
                }}</a-doption>
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
</template>
<script>
export default {
    props: ["modelValue"],
    setup() {
        const webapi = inject("webapi");
        const locale = inject("locale");
        const theme = inject("theme");
        return {
            webapi,
            locale,
            theme,
        };
    },
};
</script>
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
