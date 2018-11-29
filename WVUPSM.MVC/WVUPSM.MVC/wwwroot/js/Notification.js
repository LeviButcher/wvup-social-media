function updateNotificationNumber() {
    const ele = document.querySelector('#noti-num');
    $.ajax({
        url: `Notification/UnreadCount`,
        method: "GET"
    }).done((data) => { ele.textContent = data; });
}
updateNotificationNumber();

//Every 30s seconds update
setInterval(updateNotificationNumber, 30000);

/*
 * Setting Notifications to Read
 */
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
        updateNotificationNumber();
    });
}