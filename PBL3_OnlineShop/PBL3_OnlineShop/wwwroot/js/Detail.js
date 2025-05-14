document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('favouriteBtn');
    if (!btn) return;
    btn.addEventListener('click', function () {
        const productId = btn.getAttribute('data-product-id');
        // Chuyển hướng đến MVC controller thay vì gọi API
        window.location.href = `/Favourite/Create/${productId}`;
    });
}); 