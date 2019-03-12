<template>
  <v-layout>
    <v-flex>
      <h3>HOME</h3>
      <v-btn @click="test401">Test 401</v-btn>
    </v-flex>
  </v-layout>
</template>

<script>
export default {
  methods: {
    test401() {
      window.console.dir(this.axios);
      this.axios.post("user/test401").then(resp => {
        console.log(resp);
      });
    }
  }
};
</script>
