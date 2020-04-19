<template>
  <v-navigation-drawer fixed app clipped v-model="value">
    <v-list class avatar>
      <v-list-tile to="/">
        <v-icon>mdi-account</v-icon>
        {{ user && user.userName ? user.userName : "Anonymus" }}
      </v-list-tile>
    </v-list>
    <v-list class="ui-list">
      <v-list-tile to="/"> <v-icon>mdi-home</v-icon>Home </v-list-tile>
      <v-list-tile to="/admin">
        <v-icon>mdi-account-key</v-icon>
        Admin
      </v-list-tile>
      <v-list-tile to="/settings">
        <v-icon>mdi-settings</v-icon>
        Site Settings
      </v-list-tile>
      <v-list-tile to="/about">
        <v-icon>mdi-information</v-icon>
        About
      </v-list-tile>
      <v-list-tile to="/login">
        <v-icon>mdi-login-variant</v-icon>
        Login
      </v-list-tile>
    </v-list>
    <v-divider />
  </v-navigation-drawer>
</template>

<script>
import { mapState } from "vuex";

export default {
  props: {
    drawer: { type: Boolean, default: false }
  },
  computed: {
    ...mapState({
      user: state => state.user.user
    }),
    value: {
      get() {
        return this.drawer;
      },
      set(value) {
        this.$emit("change", value);
      }
    }
  }
};
</script>


