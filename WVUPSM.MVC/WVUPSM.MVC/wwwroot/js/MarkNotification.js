const markForms = document.querySelectorAll("[data-mark]");
if (markForms.length > 0) {
    markForms.forEach(form => form.addEventListener('submit', MarkAsRead));
}

function MarkAsRead(e) {
    e.preventDefault();
    let notificationId = this.dataset.mark;
    $.ajax({
        url: `${baseUrl}/Notification/Mark/${notificationId}`,
        method: "POST"
    }).then(() => {
        this.parentElement.parentElement.parentElement.removeChild(this.parentElement.parentElement);
        });
}