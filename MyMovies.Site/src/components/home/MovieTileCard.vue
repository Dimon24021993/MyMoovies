<template>
  <v-card hover height="100%" @click="goTo" tile>
    <v-img :src="item.pictures.filter(x => x.type == 2)[0].href">
      <span style="color:white" class="body-2">{{ item.rates }}</span>
      <span style="color:white" class="body-2"></span>
    </v-img>
    <v-card-title class="pa-2">
      <div>
        <router-link :to="`/movie/${item.id}`">
          {{ item.originalName }}
        </router-link>
        <div>
          {{ new Date(Date.parse(item.date)).getFullYear() }},
          {{ item.country }}
          <div>
            {{ item.tags.map(x => x.tagText).join(", ") }}
          </div>
        </div>
      </div>
    </v-card-title>
  </v-card>
</template>
<script>
export default {
  data() {
    return {};
  },
  methods: {
    goTo() {
      this.$router.push(`/movie/${this.item.id}`);
    }
  },
  props: {
    item: { type: Object }
  }
};
</script>
