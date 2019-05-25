import Vapi from "vuex-rest-api";

const user = new Vapi({
  state: {
    pagination: {
      entities: [],
      page: 1,
      size: 12,
      asc: true
    },
    movie: {}
  }
})
  .post({
    action: "getMovies",
    property: "pagination",
    path: "/Movies/GetMoviesPagination",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    }
  })
  .get({
    action: "getMovie",
    property: "movie",
    queryParams: true,
    path: "/Movies/GetMovie",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    }
  })
  .getStore();

export default user;
