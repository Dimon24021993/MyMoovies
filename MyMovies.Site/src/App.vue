<template>
  <v-app>
    <appNavMenu v-if="access" />
    <v-content>
      <router-view />
    </v-content>
  </v-app>
</template>

<script>
import appNavMenu from "@/components/common/app-navMenu";

export default {
  name: "App",
  components: {
    appNavMenu
  },
  created: function() {},
  computed: {
    access: {
      get() {
        return (
          !this.$route.meta.auth ||
          (this.$route.meta.auth &&
            this.$store.state.stored.user.token &&
            (!this.$route.meta.roles ||
              (this.$store.state.stored.user.roles &&
                this.$store.state.stored.user.roles.some(x =>
                  this.$route.meta.roles.includes(x)
                ))))
        );
      }
    },
    token() {
      return this.$store.state.stored.user.token;
    },
    auth() {
      return this.$route.meta.auth;
    },
    roles() {
      return this.$store.state.stored.user.roles;
    },
    includes() {
      return (
        this.$store.state.stored.user.roles &&
        this.$store.state.stored.user.roles.some(x =>
          this.$route.meta.roles.includes(x)
        )
      );
    }
  },
  data() {
    return {};
  }
};
</script>
