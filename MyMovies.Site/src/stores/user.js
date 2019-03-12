import config from "../config";
import Vapi from "vuex-rest-api";
import router from "../router";

const user = new Vapi({
        baseURL: config.baseApiUrl,
        state: {
            user: {
                comId: 0,
                token: "",
                userName: "",
                custNo: "",
                fio: "",
                roles: []
            }
        }
    })
    .post({
        action: "login",
        property: "user",
        path: "/user/login",
        onSuccess(state, payload, axios) {
            axios.defaults.headers.common["Authorization"] = "Bearer " + payload.data.token;
            state.user = payload.data;
            router.push(router.currentRoute.query.ReturnUrl ? router.currentRoute.query.ReturnUrl : "/")
        },
        onError(state, error) {
            //error
            state.error.user = error;
            state.user = {
                comId: 0,
                token: "",
                userName: "",
                custNo: "",
                fio: "",
                roles: []
            };
        }
    })
    .getStore();

// user.actions = {
//     logout() {
//         console.dir(stored.user);
//     }
// }

export default user;