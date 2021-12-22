<template>
  <a-space>
    <router-link to="/" class="c-logo">
      <a-space>
        <img :alt="modelValue.name" :src="modelValue.logo" />
        <h1>{{ modelValue.name }}</h1>
      </a-space>
    </router-link>
  </a-space>
</template>
<script>
export default {
  props: ["modelValue"],
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
