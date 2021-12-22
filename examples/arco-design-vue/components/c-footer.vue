<template>
  <a-space>
    <div v-html="modelValue.copyright"></div>
    <div v-html="modelValue.version"></div>
  </a-space>
</template>
<script>
export default {
  props: ["modelValue"],
};
</script>
