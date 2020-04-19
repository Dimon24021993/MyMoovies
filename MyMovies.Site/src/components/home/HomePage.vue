<template>
  <v-container grid-list-xl>
    <v-layout row wrap>
      <v-flex v-for="(item, i) in movies" :key="i" xs6 sm4 md3>
        <template v-if="movies && movies.length">
          <movie-card :item="item"> </movie-card>
        </template>
      </v-flex>
      <v-flex>
        <v-pagination
          v-model="currentPage"
          :length="pagination.pages"
          @input="pageChange"
        ></v-pagination>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import { mapState, mapActions } from "vuex";
import movieCard from "./MovieTileCard";

export default {
  data() {
    return {
      currentPage: 1
    };
  },
  components: {
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

