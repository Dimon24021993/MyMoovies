<template>
  <v-layout>
    <v-flex>
      <!-- 
      <v-btn @click="loadMovies({ data: { size: 10, page: 1 } })">
        Load Movies
      </v-btn> 
      -->
      <slider :items="movies" :options="options" v-if="movies && movies.length">
        <v-flex
          v-for="(item, i) in moviesWithImage"
          :key="i"
          :item="item"
          :slot="`slide${i}`"
          height="300px"
          min-width="100px"
        >
          <!-- <v-flex  /> -->
          <!-- <v-img :src="item.image" /> -->
        </v-flex>
      </slider>
      <template v-if="movies && movies.length">
        <movie-card v-for="(item, i) in movies" :key="i" :item="item">
          <!-- WithImage -->
        </movie-card>
      </template>
      <!-- <p>{{ pagination1 }}</p> -->
      <v-pagination
        v-model="currentPage"
        :length="pagination.pages"
        @input="pageChange"
      ></v-pagination>
    </v-flex>
  </v-layout>
</template>

<script>
import { mapState, mapActions } from "vuex";
import movieCard from "./MovieCard";
import slider from "../common/app-slider";

export default {
  data() {
    return {
      currentPage: 1
    };
  },
  components: {
    slider,
    movieCard
  },
  created() {
    this.loadMovies({
      data: {
        size: this.pagination.size,
        page: this.$route.params.page || this.pagination.page
      }
    });
    this.currentPage = +this.$route.params.page || 1;
  },
  computed: {
    moviesWithImage() {
      return this.movies
        .map(x => {
          var banners = x.pictures.filter(x => x.type == 2);
          if (banners.length) x.image = banners[0].href;
          return x;
        })
        .filter(x => x && !!x.image);
    },
    options() {
      return {
        spaceBetween: 0,
        loop: true,
        loopedSlides: this.pagination.size, //looped slides should be the same
        navigation: {
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev"
        }
      };
    },
    ...mapState({
      pagination: state => state.movies.pagination,
      movies: state => state.movies.pagination.entities
    })
  },
  methods: {
    pageChange($event) {
      this.$router.push("/page/" + $event);
      this.loadMovies({ data: { size: this.pagination.size, page: $event } });
    },
    ...mapActions({
      loadMovies: "movies/getMovies"
    })
  }
};
</script>

