window.a = setInterval(() => {
  if (
    document.querySelector(".all-products-loaded .nm-infload-to-top") == null
  ) {
    var b = document.querySelector(".nm-infload-btn");
    if (b) b.dispatchEvent(new Event("click"));
  } else {
    clearInterval(window.a);
  }
}, 300);

return document.querySelectorAll(".product");
