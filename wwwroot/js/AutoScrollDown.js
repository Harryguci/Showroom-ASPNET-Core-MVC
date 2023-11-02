(function () {
    var top = document.querySelector("h1").offsetTop;
    var temp = document.querySelector('#header-carousel').offsetHeight / 3 * 2
    top = top + temp
    window.scrollTo(0, top)
})()