<template>
  <div>
    <v-toolbar
      app
      clipped-left
      clipped-right
      class="ui-toolbar"
      dense
      dark
      color="primary"
    >
      <v-toolbar-side-icon @click.stop="drawerChange(!drawer)" />
      <router-link to="/">
        <img src="/img/logo.png" />
      </router-link>

      <v-toolbar-items>
        <v-btn flat to="/">
          <v-icon>mdi-home</v-icon>
          <span class="pl-2 hidden-md-and-down">Главная</span>
        </v-btn>

        <v-btn flat to="/about">About</v-btn>
      </v-toolbar-items>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-menu offset-y right>
          <v-btn flat slot="activator">
            <v-icon>mdi-account</v-icon>
            {{ user.custNo }}
            <v-icon>mdi-menu-down</v-icon>
          </v-btn>

          <v-toolbar-items>
            <v-btn flat to="/about">
              <v-icon>mdi-cart</v-icon>
            </v-btn>
          </v-toolbar-items>
          <hr />
          <v-list class="ui-list">
            <v-list-tile to="/">
              <v-list-tile-action>
                <v-icon>mdi-home</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>USER Menu</v-list-tile-title>
            </v-list-tile>

            <v-subheader>Admin</v-subheader>
            <v-list-tile to="/admin/users">
              <v-list-tile-action>
                <v-icon>mdi-account-multiple</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>Users</v-list-tile-title>
            </v-list-tile>

            <v-subheader></v-subheader>
            <v-list-tile to="/logout">
              <v-list-tile-action>
                <v-icon>mdi-power</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>logout</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-toolbar-items>
    </v-toolbar>

    <appDrawer :drawer="drawer" @change="drawerChange" />
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";
import appDrawer from "@/components/common/app-drawer";

export default {
  data() {
    return {
      drawer: false
    };
  },
  // props: {
  //   value: { type: Boolean, default: false }
  // },
  components: {
    appDrawer
  },
  methods: {
    drawerChange(value) {
      this.drawer = value;
    },
    ...mapActions({
      //   switchDrawer: "tools/switchDrawer"
    })
  },
  computed: {
    ...mapState({
      user: state => state.user.user
    })
  }
};
</script>
