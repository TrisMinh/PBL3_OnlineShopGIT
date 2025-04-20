// Home

const prevBtns = document.querySelectorAll('.prev');
const nextBtns = document.querySelectorAll('.next');
const slidesList = document.querySelectorAll('.inner-list');

function updateSlide(slides, currentSlide) {
  const offset = -currentSlide * 100;
  slides.style.transform = `translateX(${offset}%)`;
}

function autoSlide(slides, currentSlide, totalSlides, intervalTime, prevBtn, nextBtn) {
  setInterval(() => {
    if (currentSlide < totalSlides - 1) {
      currentSlide++;
    } else {
      currentSlide = 0;
    }
    updateSlide(slides, currentSlide);
    updateNavigation(currentSlide, totalSlides, prevBtn, nextBtn);
  }, intervalTime);
}

function updateNavigation(currentSlide, totalSlides, prevBtn, nextBtn) {
  if (currentSlide === 0) {
    prevBtn.classList.add('disabled');
  } else {
    prevBtn.classList.remove('disabled');
  }

  if (currentSlide === totalSlides - 1) {
    nextBtn.classList.add('disabled');
  } else {
    nextBtn.classList.remove('disabled');
  }
}

prevBtns.forEach((prevBtn, index) => {
  const nextBtn = nextBtns[index];
  const slides = slidesList[index];
  let currentSlide = 0;
  const totalSlides = slides.querySelectorAll('.slide').length;

  function updateNavigationForManual() {
    updateNavigation(currentSlide, totalSlides, prevBtn, nextBtn);
  }

  prevBtn.addEventListener('click', () => {
    if (currentSlide > 0) {
      currentSlide--;
    }
    updateSlide(slides, currentSlide);
    updateNavigationForManual();
  });

  nextBtn.addEventListener('click', () => {
    if (currentSlide < totalSlides - 1) {
      currentSlide++;
    }
    updateSlide(slides, currentSlide);
    updateNavigationForManual();
  });

  updateNavigationForManual();

  if (index === 0) {
    setTimeout(() => {
      autoSlide(slides, currentSlide, totalSlides, 5000, prevBtn, nextBtn);
    }, 0);
  } else if (index === 2) {
    setTimeout(() => {
      autoSlide(slides, currentSlide, totalSlides, 4500, prevBtn, nextBtn);
    }, 1000);
  } else if (index === 1) {
    setTimeout(() => {
      autoSlide(slides, currentSlide, totalSlides, 4000, prevBtn, nextBtn);
    }, 2000);
  }
});

// End Home

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