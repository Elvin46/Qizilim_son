

var swiper = new Swiper(".mySwiper", {
    loop: true,
    spaceBetween: 30,
    slidesPerView: 5,
    freeMode: false,
    watchSlidesProgress: false,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    thumbs: {
        swiper: swiper,
    },
});
var swiper2 = new Swiper(".mySwiper2", {
    loop: true,
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },

    thumbs: {
        swiper: swiper,
    },

});
