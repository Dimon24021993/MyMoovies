import "@babel/polyfill";
import Vue from "vue";
import "./plugins/vuetify";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import VueAxios from 'vue-axios';
import store from "./stores";
import "./scss/style.scss";
import config from '@/config';
import VueI18n from "vue-i18n";
import messages from "./locale/index";
import 'swiper/dist/css/swiper.css';

Vue.use(VueI18n);
const i18n = new VueI18n({
  locale: store.state.user.locale, // set locale
  fallbackLocale: "en",
  messages // set locale messages
});

Vue.use(VueAxios, axios);
Vue.axios.defaults.baseURL = config.baseApiUrl;
axios.defaults.headers.common.Authorization = "Bearer " + store.state.user.user.token;
axios.defaults.headers.post["Content-Type"] = "application/json";
axios.defaults.headers.get["Content-Type"] = "application/json";
axios.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    if (error.response.status == 401) {
      store.dispatch("user/userLogout", true);
      var path = router.currentRoute.path;
      var ignored = ["/login"];
      var query = !ignored.includes(path) ? {
        ReturnUrl: `${path}`
      } : "";
      if (!query)
        router.push("/login");
      else {
        router.push({
          path: "/login",
          query
        })
      }
      //self.goLogin.apply(self);
    }
    // if (error.response.status == 403) {           ///todo 403
    //   self.goLogin.apply(self);
    //   self.setLogout();
    // }
    return Promise.reject(error);
  });

Vue.config.productionTip = false;

router.beforeEach((to, from, next) => {
  if (!to.meta.auth ||
    to.meta.auth && store.state.user.user.token && (!to.meta.roles || store.state.user.user.roles && store.state.user.user.roles.some(x => to.meta.roles.includes(x)))
  ) {
    return next();
  } else {
    if (from.path == '' || from.path == '/') return next('/login')
    return next(from.path); //403
  }
})

new Vue({
  router,
  store,
  i18n,
  render: h => h(App)
}).$mount("#app");