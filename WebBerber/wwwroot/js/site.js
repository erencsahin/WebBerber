document.addEventListener("DOMContentLoaded", function () {
    var navbar = document.getElementById("mainNav");
    window.addEventListener("scroll", function () {
        if (window.scrollY > 50) { // Scroll mesafesi
            navbar.classList.add("scrolled");
        } else {
            navbar.classList.remove("scrolled");
        }
    });
});
