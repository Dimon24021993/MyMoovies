import Vue from 'vue'
import Vuex from 'vuex'
import company from './company'
import stored from './stored'
import tools from './tools'

import createPersist from 'vuex-localstorage'

Vue.use(Vuex)

company.namespaced = true;

const store = new Vuex.Store({
    modules: {
        stored,
        company,
        tools
    },
    plugins: [
        createPersist({
            namespace: "gift",
            initialState: {},
            paths: ['stored'],
            // ONE_WEEK
            expires: 7 * 24 * 60 * 60 * 1e3
        }),
    ]

});

export default store;
