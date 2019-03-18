import user from './user';
import axios from 'axios';
import router from '../router';

const stored = {

    namespaced: true,
    state: {
        itemPerPage: 25,
        locale: 'en',
        ...user.state
    },
    mutations: {
        userLogout(state, stay) {
            state.user = {
                token: ''
            };
            delete axios.defaults.headers.common["Authorization"];
            if (!stay) {
                var path = router.currentRoute.path;
                var ignored = ["/", "", "/login"];
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
        ...user.mutations

    },
    actions: {
        userLogout(state, stay) {
            state.commit('userLogout', stay);
        },
        setLocale(state, value) {
            state.commit("setLocale", value);
        },
        ...user.actions
    },

}

export default stored;