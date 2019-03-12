<template>
  <v-layout row wrap>
    <v-dialog value="true" persistent max-width="350">
      <form
        v-on:submit.prevent="
          userLogin1({ data: { comId: companyId, login, pwd } })
        "
      >
        <v-card class="ui-card">
          <v-card-title>
            <img src="/img/logo.png" height="32" /> Авторизація
          </v-card-title>
          <v-card-text>
            <v-select
              label="Компанія"
              autocomplete:true
              :items="companies"
              item-text="name"
              item-value="comId"
              v-model="companyId"
              dense
            />
            <v-text-field
              name="login"
              v-model="login"
              label="Логін"
              placeholder=" "
              autofocus
            ></v-text-field>
            <v-text-field
              name="Password"
              v-model="pwd"
              label="Пароль"
              placeholder=" "
              type="password"
            ></v-text-field>
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="grey darken-1"
              flat
              type="button"
              @click.stop="userLogout()"
              >Logout</v-btn
            >
            <v-spacer></v-spacer>
            <v-btn color="grey darken-1" flat type="reset">Очистити</v-btn>
            <v-btn color="green darken-1" flat type="submit">Вхід</v-btn>
          </v-card-actions>
        </v-card>
      </form>
    </v-dialog>
  </v-layout>
</template>

<script>
import { mapState, mapActions } from "vuex";
//import vuex from "vuex";
//getPost({ params: { id: 12 } })
export default {
  data: () => ({
    login: "",
    pwd: ""
    // dialog: true,
  }),
  created() {
    this.getAllCompanies();
  },

  computed: {
    ...mapState({
      companies: state => state.company.companies,
      comId: state => state.stored.comId
    }),
    companyId: {
      get() {
        return this.comId;
      },
      set(value) {
        this.setCompany(value);
      }
    }
  },

  methods: {
    ...mapActions({
      getAllCompanies: "company/allCompanies",
      userLogin: "stored/login",
      userLogout: "stored/userLogout",
      setCompany: "stored/setCompany"
    }),
    userLogin1(data) {
      window.console.log(this.userLogin.toString());
      this.userLogin({
        data: { comId: this.companyId, login: this.login, pwd: this.pwd }
      });
    }
  }
};
</script>
