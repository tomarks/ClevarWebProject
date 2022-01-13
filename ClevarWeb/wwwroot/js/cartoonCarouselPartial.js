console.log("Debug: Loaded cartoonCarouselPartial.js");

var mySwiper = new Swiper('.swiper-container', {
    // Optional parameters
    direction: 'horizontal',
    centeredSlides: true,
    loop: false,
    spaceBetween: 10,
    //autoplay: {
    //    delay: 30000,
    //},
    slidesPerView: 'auto',
    grabCursor: true,

    // If we need pagination
    pagination: {
        el: '.swiper-pagination',
    },

    // Navigation arrows
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },

    //// And if we need scrollbar
    //scrollbar: {
    //    el: '.swiper-scrollbar',
    //},
})


// *****************
// Cartoon Card Flip Event
// Flips the clevar-img-container if it has a parent class of active glide slide
var flipCard = function(e, callback){
    var target = $(e.target);
    var container = target.parent().closest('.swiper-slide');
    //var glideSlide = target.parent().closest('.glide__slide');
    //if (container.parent().hasClass('swiper-slide-active')) {
    if (container.hasClass('swiper-slide-active')) {
        setTimeout(function () {
            container.toggleClass('is-flipped');
        }, 50)
    }

    if (callback) {
        callback();
    }
};


var debounceFlipCard = debounce(function (e) {
    flipCard(e)
}, 500, true);
$('.swiper-slide').click(function (e) {
    debounceFlipCard(e)
});
// Returns a function, that, as long as it continues to be invoked, will not
// be triggered. The function will be called after it stops being called for
// N milliseconds. If `immediate` is passed, trigger the function on the
// leading edge, instead of the trailing.
function debounce(func, wait, immediate) {
    var timeout;

    // This is the function that is actually executed when
    // the DOM event is triggered.
    return function executedFunction() {
        // Store the context of this and any
        // parameters passed to executedFunction
        var context = this;
        var args = arguments;

        // The function to be called after 
        // the debounce time has elapsed
        var later = function () {
            // null timeout to indicate the debounce ended
            timeout = null;

            // Call function now if you did not on the leading end
            if (!immediate) func.apply(context, args);
        };

        // Determine if you should call the function
        // on the leading or trail end
        var callNow = immediate && !timeout;

        // This will reset the waiting every function execution.
        // This is the step that prevents the function from
        // being executed because it will never reach the 
        // inside of the previous setTimeout  
        clearTimeout(timeout);

        // Restart the debounce waiting period.
        // setTimeout returns a truthy value (it differs in web vs node)
        timeout = setTimeout(later, wait);

        // Call immediately if you're dong a leading
        // end execution
        if (callNow) func.apply(context, args);
    };
};