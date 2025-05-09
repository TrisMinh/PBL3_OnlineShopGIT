document.addEventListener("DOMContentLoaded", function () {
    const input = document.querySelector('input[name="ImageUpload"]');
    const previewContainer = document.getElementById('previewImages');

    if (input && previewContainer) {
        input.addEventListener('change', function (e) {
            previewContainer.innerHTML = ''; // Clear cũ
            const files = e.target.files;

            if (files) {
                Array.from(files).forEach(file => {
                    const reader = new FileReader();
                    reader.onload = function (event) {
                        const img = document.createElement('img');
                        img.src = event.target.result;
                        img.className = 'img-thumbnail me-2 mb-2';
                        img.style.maxWidth = '150px';
                        previewContainer.appendChild(img);
                    };
                    reader.readAsDataURL(file);
                });
            }
        });
    }
});

$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm deletion")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});
