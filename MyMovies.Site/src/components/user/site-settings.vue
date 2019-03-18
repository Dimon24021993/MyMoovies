<template>
  <v-layout>
    <v-flex>
      <v-select
        v-model="locale"
        :items="Object.keys($i18n.messages)"
      ></v-select>
      <v-btn v-on:click="goAway">test</v-btn>
      <router-view></router-view>
    </v-flex>
  </v-layout>
</template>

<script>
import vuex from "vuex";

export default {
  computed: {
    ...vuex.mapState({ getLocale: state => state.stored.locale }),
    locale: {
      get() {
        return this.getLocale;
      },
      set(value) {
        this.setLocale(value);
        this.$i18n.locale = value;
        this.$vuetify.lang.current = value;
      }
    }
  },
  methods: {
    ...vuex.mapActions({
      setLocale: "stored/setLocale"
    }),
    goAway: function() {
      alert(`Your language - ${this.locale}`);
    }
  }
};
</script>
