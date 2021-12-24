# Demo

1. 前后端分离
1. Web 服务器和反向代理：nginx
1. 前端：arco design vue + fetch mock
1. 后端：dotnet/java + rest + swagger + jwt + websocket

## 演示

1. [Arco Design Vue 浏览器端示例](examples/arco-design-vue/)
1. [Element Plus 浏览器端示例](examples/element-plus/)
1. [Ant Design Vue 浏览器端示例](examples/ant-design-vue/)

## 开发环境

1. vscode v1.62.3[ESLint v2.2.2]
1. node -v => v16.13.1
1. npm -v => 7.19.1
1. npm install --save-dev eslint eslint-plugin-vue
1. ./node_modules/.bin/eslint --init
1. Docker Desktop 4.2.0
1. Microsoft Visual Studio Community 2022 => dotnet 6
1. IntelliJ IDEA 2021.2 Community Edition => java 8

## frontend

1. [x] vue dynamic component
1. [x] arco design vue theme
1. [x] arco design vue locale
1. [x] fetch mock

## backend

1. [x] nginx as web server
1. [x] nginx as api gateway
1. [x] asp.net core webapi with CORS
1. [x] asp.net core websocket
1. [ ] asp.net core signalr with redis
1. [x] redis for signalr
1. [x] spring boot rest with CORS
1. [x] spring boot websocket

## 难点

1. [ ] websocket 负载均衡
1. [ ] token 权限缓存

## 引用

1. [websocket tester](https://www.piesocket.com/websocket-tester)
1. <https://nodejs.org/zh-cn/>
1. <https://www.npmjs.com/>
1. <https://cn.eslint.org/>
1. <https://eslint.vuejs.org/>
1. <https://element-plus.gitee.io/zh-CN/component/border.html>
1. <https://www.antdv.com/docs/vue/introduce-cn>
1. <https://fontawesome.com/v5.15/icons?d=gallery&p=2&m=free>
1. <https://ionic.io/ionicons>
1. <https://pro.iviewui.com/pro/introduce>
1. <https://caniuse.com/>
1. <https://docs.microsoft.com/zh-cn/azure/active-directory/develop/v2-oauth2-auth-code-flow>
1. <https://json-schema.apifox.cn/>

## 思路

1.页面加载时，layout 组件加载并实例化，后端通过 websocket 在网站或用户信息发生变动时，推送更新到前端，前端更新 layout 组件的 model。

2.用户登录后，通过更新 layout 组件的 model 更新菜单。

## 国际化

服务端需要根据请求返回支持的语言列表和当前的语言，如果当前请求的语言不在支持的列表中，则会返回默认的语言。前端不应该设置默认的语言，后端会返回当前的语言。如果前端设置的默认语言和浏览器的默认语言不一致，会导致服务端返回的资源不匹配当前的语言。

### 前端

1. 前端发送请求到服务端获取语言列表和当前语言
1. 前端存储当前语言到 localStorage 并设置前端组件的当前语言
1. 前端发送请求时，添加当前语言到 url 的 locale 部分
1. 前端以下拉列表方式切换语言时，存储当前语言到 localStorage 并根据当前页面地址刷新网页
1. 如果语言过多，可以在切换语言时跳转到新页面，选择语言后再跳转回原页面

### 后端

1. 后端根据请求自动判断语言，提供 API 供前端获取语言列表和当前语言

## token

默认使用 jwt

### 前端

前端不持久化 access token ；refresh token 只能通过服务端以 http only 的 cooke 方式写入，此 cookie 的 path 只能为刷新 token 的 url。

参考： <https://datatracker.ietf.org/doc/html/draft-ietf-oauth-browser-based-apps>

1. 前端初始化时，访问服务端刷新 access token，成功则以变量形式在内存中存储，失败则跳转到登录
1. 前端登录成功后，设置定时器定时刷新 access token ，失败则跳转到登录
1. 前端登录或刷新 token 成功时，响应结果格式相同，refresh token 都会刷新
1. refresh token 以服务端设置 cookie 的方式存储，cookie 的 path 为刷新 token 的 path，且 cookie 为 http only
1. 退出登录时，请求服务端销毁 refresh token 的 cookie
1. 前端通过监听 storage 事件的方式，实现在退出登录时清空所有浏览器所有 tab 页的 access token
1. 可以通过在服务端存储已注销且未过期的 access token 的方式防止 access token 的盗用

### 后端

1. 后端需要有 login、logout、refresh 三个接口，分别用于登录、登出和刷新
1. login 接口需要验证用户，refresh 接口需要验证 refresh token
1. login 接口和 refresh 接口成功时通过服务端将 refresh token 已 http only 的方式写入 cookie，cookie 的 path 必须设置为 refresh 的 path
