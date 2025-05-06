// Auto-close notifications after 5 seconds
function setupNotifications() {
    // Auto dismiss after delay
    setTimeout(() => {
        document.querySelectorAll('.notification-card').forEach(el => {
            dismissNotification(el);
        });
    }, 5000);

    // Close button event
    document.querySelectorAll('.notification-close').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const card = e.target.closest('.notification-card');
            dismissNotification(card);
        });
    });
}

// Dismiss notification with animation
function dismissNotification(element) {
    if (element) {
        element.style.animation = 'fadeOut 0.5s ease-out forwards';
        element.addEventListener('animationend', () => {
            element.remove();
        });
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', setupNotifications);