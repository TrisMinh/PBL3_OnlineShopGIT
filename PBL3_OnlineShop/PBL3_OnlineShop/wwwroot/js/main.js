document.addEventListener('DOMContentLoaded', function () {



    document.querySelectorAll('.slider-viewport').forEach(function (viewport, sliderIndex) {
        const list = viewport.querySelector('.inner-list');
        const slides = list.querySelectorAll('.slide');
        const slideWidth = 320; // 300px width + 20px gap
        const visibleSlides = 3;
        const totalSlides = Math.min(slides.length, 9);

        const prevBtn = document.querySelectorAll('.prev')[sliderIndex];
        const nextBtn = document.querySelectorAll('.next')[sliderIndex];

        if (totalSlides <= visibleSlides) {
            if (prevBtn) prevBtn.classList.add('disabled');
            if (nextBtn) nextBtn.classList.add('disabled');
            return;
        }

        let currentIndex = 0;
        const maxIndex = totalSlides - visibleSlides;


        function updateButtonStatus(index) {

            if (prevBtn) {
                if (index === 0) {
                    prevBtn.classList.add('disabled');
                } else {
                    prevBtn.classList.remove('disabled');
                }
            }

            if (nextBtn) {
                if (index >= maxIndex) {
                    nextBtn.classList.add('disabled');
                } else {
                    nextBtn.classList.remove('disabled');
                }
            }
        }

        function slideToIndex(index) {

            if (index < 0) index = 0;
            if (index > maxIndex) index = 0;

            list.style.transform = `translateX(-${index * slideWidth}px)`;
            currentIndex = index;

            updateButtonStatus(index);
        }

        updateButtonStatus(currentIndex);

        setInterval(() => {

            if (currentIndex >= maxIndex) {
                slideToIndex(0);
            } else {
                slideToIndex(currentIndex + 1);
            }
        }, 2000);

        if (prevBtn) {
            prevBtn.addEventListener('click', () => {
                if (currentIndex > 0) {
                    slideToIndex(currentIndex - 1);
                }
            });
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', () => {
                if (currentIndex < maxIndex) {
                    slideToIndex(currentIndex + 1);
                }
            });
        }
    });
});

// ProfileAdminEdit

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileAdminEdit

// ProfileAdminPassword

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileAdminPassword

// ProfileCustomerAddress

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileCustomerAddress

// ProfileCustomerCoupon

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileCustomerCoupon

// ProfileCustomerPassword

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileCustomerPassword

// ProfileEditCustomer

document.addEventListener('DOMContentLoaded', () => {
    const avatarImage = document.getElementById('sidebar-avatar'); // Target the avatar image by ID
    const modal = document.getElementById('avatar-modal');
    const closeModalBtn = modal.querySelector('.modal-close-btn');
    const cancelButton = modal.querySelector('.modal-button.cancel-button');
    const submitButton = modal.querySelector('.modal-button.submit-button');
    const uploadInput = document.getElementById('avatar-upload-input');
    const previewImage = document.getElementById('modal-avatar-img');
    const uploadLabel = modal.querySelector('.modal-button.upload-button'); // The label acting as upload button

    // --- Open Modal ---
    if (avatarImage) {
        avatarImage.addEventListener('click', () => {
            modal.style.display = 'flex'; // Show the modal overlay (using flex for centering)
        });
    } else {
        console.error("Sidebar avatar element not found.");
    }

    // --- Close Modal ---
    const closeModal = () => {
        modal.style.display = 'none';
        // Reset file input if needed
        uploadInput.value = '';
        // Reset preview to original avatar (optional)
        // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
    };

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeModal);
    }
    if (cancelButton) {
        cancelButton.addEventListener('click', closeModal);
    }

    // Close modal if clicking outside the content area
    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            closeModal();
        }
    });

    // --- File Upload and Preview ---
    // The hidden file input is triggered natively by clicking the associated label (due to the 'for' attribute).
    // No need for an extra JS click listener on the label.

    // Handle file selection and preview
    if (uploadInput) {
        uploadInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                }
                reader.readAsDataURL(file);
            } else {
                // Handle non-image file selection or no file selected
                console.log("No valid image file selected.");
                // Optionally reset preview
                // previewImage.src = avatarImage ? avatarImage.src : 'default-placeholder.png';
            }
        });
    }

    // --- Submit Action (Placeholder) ---
    if (submitButton) {
        submitButton.addEventListener('click', () => {
            console.log('Submit clicked. Implement actual avatar update logic here.');
            // TODO: Add logic to upload the file (e.g., using Fetch API)
            // After successful upload, update the sidebar avatar and close modal
            closeModal();
        });
    }

});

// EndProfileEditCustomer

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.filter-category-button[data-filter-type="category"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var category = btn.getAttribute('data-filter-value');
            // Nếu đang active thì bỏ chọn (về trang /Products)
            if (btn.classList.contains('active')) {
                window.location.href = "/Products";
            } else if (!category || category === "All") {
                window.location.href = "/Products";
            } else {
                window.location.href = "/Products?category=" + encodeURIComponent(category);
            }
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.filter-category-button[data-filter-type="color"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var color = btn.getAttribute('data-filter-value');
            // Nếu là All thì bỏ color khỏi query string
            if (!color || color === "All") {
                window.location.href = "/Products";
            } else {
                window.location.href = "/Products?color=" + encodeURIComponent(color);
            }
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.filter-category-button[data-filter-type="availability"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var availability = btn.getAttribute('data-filter-value');
            // Nếu là All thì bỏ availability khỏi query string
            if (!availability || availability === "All") {
                window.location.href = "/Products";
            } else {
                window.location.href = "/Products?availability=" + encodeURIComponent(availability);
            }
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.filter-category-button[data-filter-type="gender"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var gender = btn.getAttribute('data-filter-value');
            // Nếu là All thì bỏ gender khỏi query string
            if (!gender || gender === "All") {
                window.location.href = "/Products";
            } else {
                window.location.href = "/Products?gender=" + encodeURIComponent(gender);
            }
        });
    });
});