<template>
  <v-card hover height="100%" @click="$router.push(movieHref)" tile>
    <v-img :src="titleHref">
      <template v-if="item.rates && item.rates.length">
        <span
          v-if="getRate(3)"
          :style="{ color: 'white', position: 'absolute', left: 0, bottom: 0 }"
          class="body-2 rate"
        >
          Imdb:{{ getRate(3).value }}
        </span>
        <span
          v-if="getRate(4)"
          :style="{ color: 'white', position: 'absolute', right: 0, bottom: 0 }"
          class="body-2 rate"
        >
          Kinopoisk:{{ getRate(4).value }}
        </span>
      </template>
    </v-img>
    <v-card-title class="pa-2">
      <div>
        <router-link :to="movieHref">
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
  computed: {
    titleHref() {
      var a = this.item.pictures.find(x => x.type == 2);
      return a ? a.href : "";
    },
    movieHref() {
      return `/movie/${this.item.id}`;
    }
  },
  methods: {
    getRate(type) {
      var a = this.item.rates.find(x => x.rateType == type);
      return a ? a : undefined;
    }
    // goTo() {
    //   this.;
    // }
  },
  props: {
    item: { type: Object }
  }
};
</script>
<style>
.rate {
  background: #53c70fd3;
}
</style>
