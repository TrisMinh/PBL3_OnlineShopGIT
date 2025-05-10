document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('favouriteBtn');
    if (!btn) return;
    btn.addEventListener('click', function () {
        const productId = btn.getAttribute('data-product-id');
        const isActive = btn.classList.contains('active');
        fetch(`/api/favourite/toggle/${productId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ isFavourite: !isActive })
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                btn.classList.toggle('active');
                const icon = btn.querySelector('i');
                if (btn.classList.contains('active')) {
                    icon.classList.remove('far');
                    icon.classList.add('fas');
                } else {
                    icon.classList.remove('fas');
                    icon.classList.add('far');
                }
            } else {
                alert(data.message || 'Có lỗi xảy ra!');
            }
        })
        .catch(() => alert('Có lỗi xảy ra!'));
    });
}); 