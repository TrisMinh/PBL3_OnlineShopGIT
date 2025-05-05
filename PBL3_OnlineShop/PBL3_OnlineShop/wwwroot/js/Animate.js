document.addEventListener("DOMContentLoaded", function () {
    // Gắn hàm vào window để HTML gọi được
    window.changeMainImage = function (thumbnail) {
        const mainImage = document.getElementById("mainImage");
        mainImage.src = thumbnail.src;

        const thumbnails = document.querySelectorAll('.thumbnail');
        thumbnails.forEach(t => t.classList.remove('active'));
        thumbnail.classList.add('active');
    };
});
