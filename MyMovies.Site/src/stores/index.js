import Vue from 'vue'
import Vuex from 'vuex'
import stored from './stored'
import tools from './tools'
import movies from './movies'
movies.namespaced = true;

import createPersist from 'vuex-localstorage'

Vue.use(Vuex)


const store = new Vuex.Store({
    modules: {
        movies,
        stored,
        tools
    },
    plugins: [
        createPersist({
            namespace: "mymoovies",
            initialState: {},
            paths: ['stored'],
            // ONE_WEEK
            expires: 7 * 24 * 60 * 60 * 1e3
        }),
    ]

});

export default store;