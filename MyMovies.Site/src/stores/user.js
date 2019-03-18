import config from "../config";
import Vapi from "vuex-rest-api";
import router from "../router";

const user = new Vapi({
        baseURL: config.baseApiUrl,
        state: {
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

export default user;