module.exports = {
  plugins: ["vue"],
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    'eslint:recommended',
    'plugin:vue/vue3-recommended',
    "prettier"// Make sure "prettier" is the last element in this list.
  ],
  parser: "vue - eslint - parser",
  parserOptions: {
    ecmaVersion: 12,
    sourceType: 'module',
  },
  rules: {
  },
};
