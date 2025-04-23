const prevBtns = document.querySelectorAll('.prev');
const nextBtns = document.querySelectorAll('.next');
const slidesList = document.querySelectorAll('.inner-list');

function updateSlide(slides, currentSlide) {
  const slide = document.querySelector('.slide');
  const width = slide.offsetWidth;
  const slide2 = document.querySelector('.section-two');
  const width2 = slide2.offsetWidth;
  const offset = -currentSlide * width / width2 * 100;
  slides.style.transform = `translateX(${offset}%)`;
}

function autoSlide(slides, currentSlideRef, totalSlides, intervalTime, prevBtn, nextBtn) {
  return setInterval(() => {
    if (currentSlideRef.value < totalSlides - 1) {
      currentSlideRef.value++;
    } else {
      currentSlideRef.value = 0;
    }
    updateSlide(slides, currentSlideRef.value);
    updateNavigation(currentSlideRef.value, totalSlides, prevBtn, nextBtn);
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
  const currentSlideRef = { value: 0 };
  const totalSlides = slides.querySelectorAll('.list').length - 2;

  function updateNavigationForManual() {
    updateNavigation(currentSlideRef.value, totalSlides, prevBtn, nextBtn);
  }

  prevBtn.addEventListener('click', () => {
    if (currentSlideRef.value > 0) {
      currentSlideRef.value--;
    }
    updateSlide(slides, currentSlideRef.value);
    updateNavigationForManual();
  });

  nextBtn.addEventListener('click', () => {
    if (currentSlideRef.value < totalSlides - 1) {
      currentSlideRef.value++;
    }
    updateSlide(slides, currentSlideRef.value);
    updateNavigationForManual();
  });

  updateNavigationForManual();

  let intervalTime;
  if (index === 0) intervalTime = 5000;
  else if (index === 1) intervalTime = 4000;
  else if (index === 2) intervalTime = 4500;

  setTimeout(() => {
    autoSlide(slides, currentSlideRef, totalSlides, intervalTime, prevBtn, nextBtn);
  }, index * 1000);

  const mediaQuery = window.matchMedia("(max-width: 992px)");

  function onMediaChange() {
    updateSlide(slides, currentSlideRef.value);
  }

  mediaQuery.addEventListener("change", onMediaChange);
  window.addEventListener("resize", onMediaChange);
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