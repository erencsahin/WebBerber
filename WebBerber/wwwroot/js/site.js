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
document.addEventListener("DOMContentLoaded", function () {
    const loadingAnimation = document.getElementById("loading-animation");

    // Tüm bağlantılara tıklama olayını yakala
    document.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", function (event) {
            if (!link.href.includes("#") && link.target !== "_blank") {
                event.preventDefault(); // Varsayılan tıklamayı durdur
                loadingAnimation.classList.add("show"); // Animasyonu yumuşakça göster

                // Yeni sayfaya yönlendir
                setTimeout(() => {
                    window.location.href = link.href;
                }, 600); // Animasyonun tam görünmesi için gecikme süresi
            }
        });
    });
});
