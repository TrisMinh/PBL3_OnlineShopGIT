// JavaScript to handle section display based on navigation clicks
function showSection(sectionId) {
    // Hide all sections
    const sections = document.querySelectorAll('.profile-content-wrapper8');
    sections.forEach((section) => {
        section.classList.remove('section-active');
    });

    // Show the clicked section
    const section = document.getElementById(sectionId);
    if (section) {
        section.classList.add('section-active');
    }
}

// Handle avatar upload functionality
const avatarUploadInput = document.getElementById('avatar-upload-input');
const avatarPreview = document.getElementById('modal-avatar-img');
const avatarModal = document.getElementById('avatar-modal');
const closeModalButton = document.querySelector('.modal-close-btn5');

// Open avatar change modal
document.getElementById('sidebar-avatar').addEventListener('click', () => {
    avatarModal.style.display = 'block';
});

// Close avatar change modal
closeModalButton.addEventListener('click', () => {
    avatarModal.style.display = 'none';
});

// Handle avatar image upload
avatarUploadInput.addEventListener('change', (e) => {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (event) {
            avatarPreview.src = event.target.result;
        };
        reader.readAsDataURL(file);
    }
});

// Handle submit button in modal (Save avatar)
document.querySelector('.submit-button5').addEventListener('click', () => {
    // Add logic to save the new avatar (if necessary, e.g., send the file to the server)
    alert('Avatar has been updated!');
    avatarModal.style.display = 'none';
});

// Handle cancel button in modal (Close without saving)
document.querySelector('.cancel-button5').addEventListener('click', () => {
    avatarModal.style.display = 'none';
});

// Handle profile form submit
document.querySelector('.submit-button8').addEventListener('click', (event) => {
    event.preventDefault();
    const username = document.getElementById('username').value;
    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
    const phone = document.getElementById('phone').value;
    const gender = document.querySelector('input[name="gender"]:checked').value;
    const dobDay = document.getElementById('dob-day').value;
    const dobMonth = document.getElementById('dob-month').value;
    const dobYear = document.getElementById('dob-year').value;

    // You can add validation here and send data to the server if necessary
    alert('Profile updated successfully!');
});

// Handle change password form submit
document.querySelector('.submit-button7').addEventListener('click', (event) => {
    event.preventDefault();
    const currentPassword = document.getElementById('current-password').value;
    const newPassword = document.getElementById('new-password').value;
    const confirmPassword = document.getElementById('confirm-password').value;

    // You can add password validation here and send data to the server if necessary
    if (newPassword === confirmPassword) {
        alert('Password changed successfully!');
    } else {
        alert('Passwords do not match!');
    }
});
