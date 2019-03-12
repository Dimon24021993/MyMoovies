<template>
  <div>
    <v-toolbar app clipped-left clipped-right class="ui-toolbar" dense dark color="primary">
      <v-toolbar-side-icon @click.stop="drawer = !drawer"/>
      <router-link to="/">
        <img src="/img/logo.png">
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
            {{user.custNo}}
            <v-icon>mdi-menu-down</v-icon>
          </v-btn>

          <v-toolbar-items>
            <v-btn flat to="/about">
              <v-icon>mdi-cart</v-icon>
            </v-btn>
          </v-toolbar-items>
          <hr>
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

    <v-navigation-drawer fixed app clipped v-model="drawer">
      <v-list class avatar>
        <v-list-tile to="/">
          <v-icon>mdi-home</v-icon>
          {{user.custNo}}
          <br>
          {{user.fio}}
        </v-list-tile>
      </v-list>
      <v-list class="ui-list">
        <v-list-tile to="/">
          <v-icon>mdi-home</v-icon>home
        </v-list-tile>
        <v-list-tile to="/about">about</v-list-tile>
        <v-list-tile to="/admin">admin</v-list-tile>
        <v-list-tile to="/login">login</v-list-tile>
      </v-list>
      <v-divider/>
    </v-navigation-drawer>
    <!-- <Drawer v-model="drawer"/> -->
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";
// import Drawer from "@/components/nav/Drawer";

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
    // Drawer
  },
  methods: {
    ...mapActions({
      //   switchDrawer: "tools/switchDrawer"
    })
  },
  computed: {
    ...mapState({
      user: state => state.stored.user
    })
  }
};
</script>
