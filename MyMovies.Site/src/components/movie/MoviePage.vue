<template>
  <v-container grid-list-md>
    <v-layout row wrap v-if="movie">
      <v-flex xs2 v-if="movie.pictures">
        <v-img
          :src="movie.pictures.filter(x => x.type == 2)[0].href"
          aspect-ratio
        ></v-img>

        <!-- <p>{{ movie }}</p> -->
      </v-flex>
      <v-flex xs10 v-if="movie.descriptions">
        <p>{{ movie.descriptions[0].descriptionText }}</p>
      </v-flex>
      <v-flex v-if="movie.pictures" xs4>
        <slider
          :options="options"
          :items="movie.pictures.filter(x => ![1, 2].includes(x.type))"
        >
          <v-flex
            v-for="(picture, i) in movie.pictures.filter(
              x => ![1, 2].includes(x.type)
            )"
            :key="i"
            :slot="`slide${i}`"
          >
            <v-img :src="picture.href"></v-img>
          </v-flex>
        </slider>
      </v-flex>
      <v-flex xs8 v-if="movie.items">
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
    return {};
  },
  components: {
    slider
  },
  created() {
    this.loadMovie({ params: { movieId: this.$route.params.movieId } });
  },
  computed: {
    videoHref() {
      return this.movie.items.filter(
        x =>
          x.itemType == 0 &&
          [1, 2, 3, 4].includes(x.value.split(".").reverse()[0].length)
      )[0].value;
    },
    ...mapState({
      movie: state => state.movies.movie
    }),
    options() {
      return {
        spaceBetween: 0,
        loop: true,
        loopedSlides: this.movie.pictures.filter(x => ![1, 2].includes(x.type))
          .length, //looped slides should be the same
        navigation: {
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev"
        }
      };
    }
  },
  methods: {
    ...mapActions({
      loadMovie: "movies/getMovie"
    })
  }
};
</script>
