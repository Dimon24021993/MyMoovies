<template>
  <v-navigation-drawer fixed app clipped v-model="value">
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
</template>

<script>
import { mapState } from "vuex";

export default {
  props: {
    value: { type: Boolean, default: false }
  },
  computed: {
    ...mapState({
      //show: state => state.tools.drawer
    })
  }
};
</script>


