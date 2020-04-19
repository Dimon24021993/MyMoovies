<template v-if="items.length && options">
  <swiper :options="options" class="gallery">
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
      if (item && item.image) return `background-image: url(${item.image});`;
      return "";
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