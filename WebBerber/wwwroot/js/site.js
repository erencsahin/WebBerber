/*animasyonlar için aos kullanımı */
document.addEventListener("DOMContentLoaded", function () {
    AOS.init({
        duration: 800,  // Animasyon süresi (ms)
        offset: 100,    // Tetiklenme sınırı
        once: true      // Sadece bir kez çalıştır
    });
});
document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.querySelector('.navbar');
    if (navbar) {
        const navbarHeight = navbar.offsetHeight;
        document.body.style.paddingTop = navbarHeight/2 + "px";
    }
});
