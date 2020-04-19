import Vue from 'vue'
import Vuex from 'vuex'
import user from './user'
import tools from './tools'
import movies from './movies'

import createPersist from 'vuex-localstorage'

Vue.use(Vuex)


const store = new Vuex.Store({
    modules: {
        movies,
        user,
        tools
    },
    plugins: [
        createPersist({
            namespace: "mymoovies",
            initialState: {},
            paths: ['user'],
            // ONE_WEEK
            expires: 7 * 24 * 60 * 60 * 1e3
        }),
    ]

});

export default store;