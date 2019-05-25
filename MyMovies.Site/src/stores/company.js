import config from "../config";
import Vapi from "vuex-rest-api";

const company = new Vapi({
    baseURL: config.baseApiUrl,
    state: {
        companies: []
    }
})
    .post({
        action: "allCompanies",
        property: "companies",
        path: "/user/getCompanies"
    })
    .getStore();

export default company;
