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

    // Xử lý highlight nav item
    const navItems = document.querySelectorAll('.profile-nav-item8');
    navItems.forEach((item) => {
        item.classList.remove('active8');
    });
    // Tìm nav item chứa sectionId
    navItems.forEach((item) => {
        if (item.getAttribute('onclick') && item.getAttribute('onclick').includes(sectionId)) {
            item.classList.add('active8');
        }
    });
}

// Handle avatar upload functionality
const avatarUploadInput = document.getElementById('avatar-upload-input');
const avatarPreview = document.getElementById('modal-avatar-img');
const avatarModal = document.getElementById('avatar-modal');
const closeModalButton = document.querySelector('.modal-close-btn5');

const sidebarAvatar = document.getElementById('sidebar-avatar');
if (sidebarAvatar && avatarModal) {
    sidebarAvatar.addEventListener('click', () => {
    avatarModal.style.display = 'block';
});
}
if (closeModalButton && avatarModal) {
closeModalButton.addEventListener('click', () => {
    avatarModal.style.display = 'none';
});
}
if (avatarUploadInput && avatarPreview) {
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
}
const submitButton5 = document.querySelector('.submit-button5');
if (submitButton5 && avatarModal) {
    submitButton5.addEventListener('click', () => {
    alert('Avatar has been updated!');
    avatarModal.style.display = 'none';
});
}
const cancelButton5 = document.querySelector('.cancel-button5');
if (cancelButton5 && avatarModal) {
    cancelButton5.addEventListener('click', () => {
    avatarModal.style.display = 'none';
});
}

// Handle profile form submit
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, setting up event listeners');
    
    // Thêm sự kiện cho trường Phone để ẩn thông báo lỗi khi người dùng thay đổi giá trị
    const phoneInput = document.getElementById('phone');
    if (phoneInput) {
        phoneInput.addEventListener('input', function() {
            const phoneError = document.getElementById('phone-error');
            if (phoneError) {
                phoneError.style.display = 'none';
            }
        });
    }
    
    // Thêm sự kiện cho trường Email để ẩn thông báo lỗi khi người dùng thay đổi giá trị
    const emailInput = document.getElementById('email');
    if (emailInput) {
        emailInput.addEventListener('input', function() {
            const emailError = document.getElementById('email-error');
            if (emailError) {
                emailError.style.display = 'none';
            }
        });
    }
    
    // Thêm sự kiện cho các trường ngày tháng năm
    const dobDay = document.getElementById('dob-day');
    const dobMonth = document.getElementById('dob-month');
    const dobYear = document.getElementById('dob-year');
    const dateError = document.getElementById('date-error');
    
    // Hàm kiểm tra ngày tháng hợp lệ
    function isValidDate(day, month, year) {
        // Nếu chưa chọn đủ thông tin, coi như hợp lệ (sẽ có required validation riêng)
        if (!day || !month || !year) return true;
        
        day = parseInt(day);
        month = parseInt(month);
        year = parseInt(year);
        
        // Kiểm tra ngày hợp lệ theo tháng
        if (month === 2) { // Tháng 2
            // Kiểm tra năm nhuận
            const isLeapYear = ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
            if (isLeapYear) {
                return day <= 29;
            } else {
                return day <= 28;
            }
        } else if ([4, 6, 9, 11].includes(month)) { // Tháng 4, 6, 9, 11 có 30 ngày
            return day <= 30;
        } else { // Các tháng khác có 31 ngày
            return day <= 31;
        }
    }
    
    // Hàm kiểm tra và hiển thị lỗi ngày tháng
    function validateDate() {
        if (!dobDay || !dobMonth || !dobYear) return;
        
        const day = dobDay.value;
        const month = dobMonth.value;
        const year = dobYear.value;
        
        if (!isValidDate(day, month, year)) {
            if (dateError) {
                dateError.style.display = 'block';
                dateError.textContent = `Ngày ${day} tháng ${month} không hợp lệ!`;
            }
        } else {
            if (dateError) {
                dateError.style.display = 'none';
            }
        }
    }
    
    // Gắn sự kiện cho các dropdown ngày tháng năm
    if (dobDay) dobDay.addEventListener('change', validateDate);
    if (dobMonth) dobMonth.addEventListener('change', validateDate);
    if (dobYear) dobYear.addEventListener('change', validateDate);
    
    const submitButton = document.querySelector('.submit-button8');
    console.log('Submit button found:', submitButton);
    
    if (submitButton) {
        submitButton.addEventListener('click', function() {
            console.log('Submit button clicked');
            
    const username = document.getElementById('username').value;
    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
            const phone = document.getElementById('phone').value || '';
    const gender = document.querySelector('input[name="gender"]:checked').value;
            const dobDay = document.getElementById('dob-day').value || '1';
            const dobMonth = document.getElementById('dob-month').value || '1';
            const dobYear = document.getElementById('dob-year').value || new Date().getFullYear().toString();
            const province = document.getElementById('province').value || '';
            const district = document.getElementById('district').value || '';
            const specificAddress = document.getElementById('specific-address').value || '';

            console.log('Form data collected:', { 
                username, name, email, phone, gender, 
                dob: `${dobDay}/${dobMonth}/${dobYear}`,
                address: `${province} / ${district} / ${specificAddress}`
            });

            // Kiểm tra email hợp lệ
            if (email && !email.includes('@')) {
                // Hiển thị thông báo lỗi bên dưới textbox
                const emailError = document.getElementById('email-error');
                if (emailError) {
                    emailError.style.display = 'block';
                    // Tập trung vào trường email
                    document.getElementById('email').focus();
                }
                return;
            } else {
                // Ẩn thông báo lỗi nếu đã hợp lệ
                const emailError = document.getElementById('email-error');
                if (emailError) {
                    emailError.style.display = 'none';
                }
            }

            // Kiểm tra số điện thoại (10-11 số)
            if (phone && (phone.length < 10 || phone.length > 11)) {
                // Hiển thị thông báo lỗi bên dưới textbox
                const phoneError = document.getElementById('phone-error');
                if (phoneError) {
                    phoneError.style.display = 'block';
                    // Tập trung vào trường số điện thoại
                    document.getElementById('phone').focus();
                }
                return;
            } else {
                // Ẩn thông báo lỗi nếu đã hợp lệ
                const phoneError = document.getElementById('phone-error');
                if (phoneError) {
                    phoneError.style.display = 'none';
                }
            }
            
            // Kiểm tra ngày tháng hợp lệ
            if (!isValidDate(dobDay, dobMonth, dobYear)) {
                const dateError = document.getElementById('date-error');
                if (dateError) {
                    dateError.style.display = 'block';
                    dateError.textContent = `Ngày ${dobDay} tháng ${dobMonth} không hợp lệ!`;
                    // Focus vào dropdown ngày
                    document.getElementById('dob-day').focus();
                }
                return;
            }

            // Định dạng địa chỉ: tỉnh / huyện / địa chỉ cụ thể
            const fullAddress = province && district ? 
                `${province} / ${district} / ${specificAddress}` : specificAddress;

            // Tạo đối tượng FormData để gửi dữ liệu
            const formData = new FormData();
            formData.append('UserName', username);
            formData.append('Name', name);
            formData.append('Email', email);
            formData.append('PhoneNumber', phone);
            formData.append('Gender', gender);
            formData.append('Day', dobDay);
            formData.append('Month', dobMonth);
            formData.append('Year', dobYear);
            formData.append('Address', fullAddress);

            console.log('Sending AJAX request to /Account/UpdateProfile');
            
            // Gửi request cập nhật profile
            fetch('/Account/UpdateProfile', {
                method: 'POST',
                body: formData
            })
            .then(response => {
                console.log('Response received:', response);
                if (response.ok) {
                    showProfileToast('Cập nhật thông tin thành công!', true);
                    setTimeout(() => window.location.reload(), 1500);
                } else {
                    showProfileToast('Cập nhật thông tin thất bại. Vui lòng thử lại!', false);
                }
            })
            .catch(error => {
                console.error('Fetch error:', error);
                showProfileToast('Đã xảy ra lỗi. Vui lòng thử lại.', false);
            });
        });
    } else {
        console.error('Submit button not found!');
    }

    // Handle change password button
    const changePasswordBtn = document.querySelector('.change-password-btn7');
    if (changePasswordBtn) {
        changePasswordBtn.addEventListener('click', function() {
            console.log('Change password button clicked');
    const currentPassword = document.getElementById('current-password').value;
    const newPassword = document.getElementById('new-password').value;
    const confirmPassword = document.getElementById('confirm-password').value;

            // Kiểm tra dữ liệu đầu vào 
            if (!currentPassword || !newPassword || !confirmPassword) {
                showProfileToast('Vui lòng nhập đầy đủ thông tin', false);
                return;
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (newPassword !== confirmPassword) {
                showProfileToast('Mật khẩu mới và xác nhận mật khẩu không khớp', false);
                return;
            }

            // Kiểm tra độ dài mật khẩu mới
            if (newPassword.length < 6) {
                showProfileToast('Mật khẩu mới phải có ít nhất 6 ký tự', false);
                return;
            }

            // Tạo dữ liệu để gửi đi
            const passwordData = {
                currentPassword: currentPassword,
                newPassword: newPassword,
                confirmPassword: confirmPassword
            };

            // Gửi request đổi mật khẩu
            fetch('/Account/ChangePassword', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(passwordData)
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // Xóa dữ liệu đã nhập
                    document.getElementById('current-password').value = '';
                    document.getElementById('new-password').value = '';
                    document.getElementById('confirm-password').value = '';
                    
                    // Hiển thị thông báo thành công
                    showProfileToast(data.message, true);
    } else {
                    // Hiển thị thông báo lỗi
                    showProfileToast(data.message, false);
                }
            })
            .catch(error => {
                console.error('Lỗi khi đổi mật khẩu:', error);
                showProfileToast('Đã xảy ra lỗi khi đổi mật khẩu', false);
            });
        });
    }
});

// Thêm hàm toast Bootstrap
function showProfileToast(message, isSuccess) {
    const toastEl = document.getElementById('profileToast');
    const toastMsg = document.getElementById('profileToastMsg');
    toastMsg.textContent = message;
    toastEl.classList.remove('text-bg-success', 'text-bg-danger');
    toastEl.classList.add(isSuccess ? 'text-bg-success' : 'text-bg-danger');
    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}
