import axios1 from 'axios';
import router from '../router';
import Vapi from "vuex-rest-api";

const api = new Vapi({
    state: {
      itemPerPage: 25,
      locale: 'en',
      user: {}
    }
  })
  .post({
    action: "login",
    property: "user",
    path: "/account/login",
    onSuccess(state, payload, axios) {
      axios.defaults.headers.common["Authorization"] = "Bearer " + payload.data.token;
      state.user = payload.data;
      router.push(router.currentRoute.query.ReturnUrl ? router.currentRoute.query.ReturnUrl : "/");
    },
    onError(state, error) {
      //error
      state.error.user = error;
      state.user = {};
    }
  })
  .getStore();


const user = {
  namespaced: true,
  state: api.state,
  mutations: {
    userLogout(state, stay) {
      state.user = {
        token: ''
      };
      delete axios1.defaults.headers.common.Authorization;
      if (!stay) {
        var path = router.currentRoute.path;
        var ignored = ["/", "", "/login", "/logout"];
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
      }
    },
    setLocale(state, value) {
      state.locale = value;
    },
    ...api.mutations

  },
  actions: {
    userLogout(state, stay) {
      state.commit('userLogout', stay);
    },
    setLocale(state, value) {
      state.commit("setLocale", value);
    },
    ...api.actions
  },

}

export default user;