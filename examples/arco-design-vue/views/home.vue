<template>
    <c-layout>
        <div
            style="
                padding: 1em;
                box-sizing: border-box;
                width: 100%;
                height: 100%;
            "
        >
            <a-tabs lazy-load type="card">
                <a-tab-pane
                    v-for="group in arcoComponents"
                    :key="group.name"
                    :title="group.name"
                >
                    <a-tabs lazy-load type="card" position="left">
                        <a-tab-pane
                            v-for="item in group.children"
                            :key="item.key"
                            :title="item.name"
                        >
                            <a-card>
                                <c-iframe
                                    :style="style"
                                    :src="getSrc(item.key, item.path)"
                                ></c-iframe>
                            </a-card>
                        </a-tab-pane>
                    </a-tabs>
                </a-tab-pane>
            </a-tabs>
        </div>
    </c-layout>
</template>
<script>
export default {
    setup() {
        const arcoComponents = ref([]);
        const init = () => {
            fetch("api/mock/arco.json")
                .then((o) => o.json())
                .then((o) => (arcoComponents.value = o));
        };
        onMounted(init);
        return {
            arcoComponents,
            init,
            getSrc(name, category) {
                return `https://arco.design/vue/${
                    category ?? "component"
                }/${name}`;
            },
        };
    },
};
</script>
<style>
.arco-tabs,
.arco-tabs-content-list,
.arco-tabs-content-item,
.arco-tabs-pane,
.arco-card,
.arco-card-body,
.iframe-container {
    height: 100%;
}

.arco-tabs-content {
    height: calc(100% - 40px);
}
</style>
