<template>
  <v-container grid-list-md>
    <v-layout row wrap v-if="movie">
      <v-flex xs4 md3 v-if="movie.pictures">
        <v-img
          :src="movie.pictures.find(x => x.type == 2).href"
          aspect-ratio
        ></v-img>
        <v-divider class="py-2"></v-divider>
        <template v-if="movie.rates && movie.rates.length">
          <div v-if="getRate(3)">
            Imdb: <strong>{{ getRate(3).value }}</strong>
          </div>
          <div v-if="getRate(4)">
            Kinopoisk: <strong>{{ getRate(4).value }}</strong>
          </div>
        </template>
        <!-- <p>{{ movie }}</p> -->
      </v-flex>
      <v-flex xs8 md9 v-if="movie.descriptions" @click="full = !full">
        <p class="body-2">{{ movie.descriptions[0].movieName }}</p>
        <span>
          {{
            full
              ? movie.descriptions[0].descriptionText
              : movie.descriptions[0].descriptionText.slice(0, 190)
          }}
        </span>
        <span v-if="!full" flat style="color:blue; cursor: pointer;">
          {{ $t("More text") }}
        </span>
      </v-flex>
      <v-flex v-if="movie.pictures" xs12 md4>
        <slider v-if="screens.length" :options="options" :items="screens">
          <div v-for="(picture, i) in screens" :key="i" :slot="`slide${i}`">
            <v-img :src="picture.href"></v-img>
          </div>
        </slider>
      </v-flex>
      <v-flex xs8 v-if="movie.items && videoHref">
        <video :src="videoHref" controls="controls" />
      </v-flex>
    </v-layout>
  </v-container>
</template>
<script>
import { mapState, mapActions } from "vuex";
import slider from "../common/app-slider";

export default {
  data() {
    return {
      full: false
    };
  },
  components: {
    slider
  },
  created() {
    this.loadMovie({ params: { movieId: this.$route.params.movieId } });
  },
  computed: {
    screens() {
      return this.movie.pictures.filter(x => ![1, 2].includes(x.type));
    },
    videoHref() {
      var a = this.movie.items.find(
        x =>
          x.itemType == 0 &&
          [1, 2, 3, 4].includes(x.value.split(".").reverse()[0].length)
      );
      return a ? a.value : "";
    },
    ...mapState({
      movie: state => state.movies.movie
    }),
    options() {
      return {
        spaceBetween: 0,
        loop: this.screens.length > 1,
        loopedSlides: this.screens.length, //looped slides should be the same
        slidesPerView: 1,
        navigation: {
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev"
        }
      };
    }
  },
  methods: {
    getRate(type) {
      var a = this.movie.rates.find(x => x.rateType == type);
      return a ? a : undefined;
    },
    ...mapActions({
      loadMovie: "movies/getMovie"
    })
  }
};
</script>
