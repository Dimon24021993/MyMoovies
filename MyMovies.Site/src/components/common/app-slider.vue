<template>
  <swiper :options="options" class="gallery" v-if="items.length">
    <swiper-slide
      v-for="(item, index) in items"
      :key="index"
      :style="getStyle(item)"
    >
      <slot :name="`slide${index}`"> </slot>
    </swiper-slide>
    <div
      class="swiper-button-next swiper-button-white"
      slot="button-next"
    ></div>
    <div
      class="swiper-button-prev swiper-button-white"
      slot="button-prev"
    ></div>
  </swiper>
</template>
<script>
import { swiper, swiperSlide } from "vue-awesome-swiper";

export default {
  computed: {
    // options() {
    //   return {};
    // }
  },
  components: {
    swiper,
    swiperSlide
  },
  methods: {
    getStyle(item) {
      if (!item) return "";
      return `background-image: url(${item.image});`;
    }
  },
  props: {
    items: {
      type: Array,
      default: function() {
        return [];
      }
    },
    options: { type: Object }
  }
};
</script>
<style>
/* .gallery {
  height: 700px !important;
} */
</style>