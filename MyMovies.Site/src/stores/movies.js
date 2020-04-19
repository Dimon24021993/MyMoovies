import Vapi from "vuex-rest-api";

const api = new Vapi({
    state: {
        pagination: {
            entities: [],
            page: 1,
            size: 12,
            asc: true
        },
        movie: {}
    }
}).post({
    action: "getMovies",
    property: "pagination",
    path: "/Movies/GetMoviesPagination",

}).get({
    action: "getMovie",
    property: "movie",
    queryParams: true,
    path: "/Movies/GetMovie",

}).getStore();


export default {
    namespaced: true,
    state: api.state,
    getters: api.getters,
    mutations: api.mutations,
    actions: api.actions
};