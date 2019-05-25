import Vue from 'vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import en from "vuetify/lib/locale/en";
import uk from "vuetify/lib/locale/uk";
import ru from "vuetify/lib/locale/ru";

Vue.use(Vuetify, {
  theme: {
    primary: "#141414"
  },
  // theme: {
  //   primary: "#223A5E",// "#7F4145", //"#2A3244",
  //   secondary: "#424242",
  //   accent: "#82B1FF",
  //   error: "#FF5252",
  //   info: "#2196F3",
  //   success: "#4CAF50",
  //   warning: "#FFC107"
  // },
  customProperties: false,
  iconfont: 'mdi',
  lang: {
    locales: {
      en,
      ru,
      uk
    },
    current: 'en'
  },
})