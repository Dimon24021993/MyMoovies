<template>
  <v-layout row wrap>
    <v-dialog value="true" persistent max-width="350">
      <form v-on:submit.prevent="userLogin({ data: { login, password } })">
        <v-card class="ui-card">
          <v-card-title>
            <img src="/img/logo.png" height="32" /> Авторизація
          </v-card-title>
          <v-card-text>
            <v-text-field
              name="login"
              v-model="login"
              label="Логін"
              placeholder=" "
              autofocus
            ></v-text-field>
            <v-text-field
              name="Password"
              v-model="password"
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
    password: ""
    // dialog: true,
  }),
  created() {},

  methods: {
    ...mapActions({
      userLogin: "stored/login",
      userLogout: "stored/userLogout"
    }),
    userLogin1(data) {
      this.userLogin({
        data: { login: this.login, password: this.password }
      });
    }
  }
};
</script>
