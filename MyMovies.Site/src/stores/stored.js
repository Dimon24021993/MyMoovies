import user from './user';
import axios from 'axios';
import router from '../router';

const stored = {

    namespaced: true,
    state: {
        item_card: false,
        item_pages: 25,
        content_pages_: 25,
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
        ...user.mutations

    },
    actions: {
        userLogout(state, stay) {
            state.commit('userLogout', stay);
        },
        ...user.actions
    },

}

export default stored;