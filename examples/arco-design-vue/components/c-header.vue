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
        <a-dropdown @select="webapi.change" trigger="hover">
            <a-button type="text" class="c-dropdown-btn"
                >{{ webapi.current }}&nbsp;<icon-down></icon-down
            ></a-button>
            <template #content>
                <a-doption
                    v-for="item in webapi.items"
                    :value="item.name"
                    :class="webapi.current === item ? 'current' : ''"
                    >{{ item }}</a-doption
                >
            </template>
        </a-dropdown>
        <a-dropdown @select="locale.change" trigger="hover">
            <a-button type="text" class="c-dropdown-btn"
                >{{ locale.getNativeName(locale.current) }}&nbsp;<icon-down
                ></icon-down>
            </a-button>
            <template #content>
                <a-doption
                    v-for="item in locale.items"
                    :value="item.name"
                    :class="locale.current === item.name ? 'current' : ''"
                    >{{ item.nativeName }}</a-doption
                >
            </template>
        </a-dropdown>
        <a-dropdown @select="theme.change" trigger="hover">
            <a-button type="text" class="c-dropdown-btn"
                >{{ $t("message.theme." + theme.current) }}&nbsp;<icon-down
                ></icon-down
            ></a-button>
            <template #content>
                <a-doption
                    v-for="item in theme.items"
                    :value="item"
                    :class="theme.current === item ? 'current' : ''"
                    >{{ $t("message.theme." + item) }}</a-doption
                >
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
.c-dropdown-btn,
.c-dropdown-btn:hover {
    color: var(--color-text-1);
    background-color: transparent;
}
</style>
