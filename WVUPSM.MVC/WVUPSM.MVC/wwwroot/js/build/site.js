let buttons = document.querySelectorAll('[data-post]');
buttons.forEach(button => button.addEventListener('click', toggleCommentForm));

function toggleCommentForm() {
    console.dir(this);
    let post = this.dataset.post;
    let commentForm = document.querySelector(`#comment-form${post}`);
    commentForm.classList.toggle('post-comment-form-active');
}
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
//TODO: Make Scroll loader responsible for skip take
/*
 * scrollElement
 * insertIntoElement => element to add new content to
 * render => function that can take in json and output HTML elements
 * apiCall => string of the api to call for data
 * scrollTop => bool: true if we should load content based on top scrolled
 * */
function scrollLoader(scrollElement, insertIntoElement, render, apiURL, skipStart, take, scrollTop,) {
    let skip = skipStart;

    scrollElement.addEventListener('scroll', (event) => {
        let ele = event.target;
        let addData = false;

        if (scrollTop) {
            if (ele.scrollTop === 0) {
                addData = true;
            }
        }
        else {
            if (ele.scrollHeight - ele.scrollTop === ele.clientHeight) {
                addData = true;
            }
        }

        if (addData) {
            CallApi(apiURL, skip, take)
                .then(data => {
                    return data.map(datum => {
                        return render(datum);
                    });
                })
                .then(generatedHTMLArr => {
                    generatedHTMLArr.forEach(html => {
                        insertIntoElement.appendChild(htmlToElement(html));
                    });
                })
                .then(() => { skip = skip + take; });
        }
    });
}

async function CallApi(apiURL, skip, take) {
    return $.ajax({
        url: `${baseUrl}${apiURL}?skip=${skip}&take=${take}`,
        method: "GET"
    }).done((data) => { return data; });
}
/*
    Drawer Button Functionality
*/

const button = document.querySelector("#drawer-button");
const mainContent = document.querySelector("main");
const drawer = document.querySelector("#drawer");
const footer = document.querySelector(".site-footer");

/*
 * Clear Annoucements 
 * 
*/ 
function clearAnnouncements() {
    let announcementSection = document.querySelector('.section-announcement');
    console.dir(announcementSection);
    announcementSection.parentNode.removeChild(announcementSection);
}

/*
 * 
 * Converts string to html node
 * Credit to Mark Amery: https://stackoverflow.com/questions/494143/creating-a-new-dom-element-from-an-html-string-using-built-in-dom-methods-or-pro
*/
function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim(); // Never return a text node of whitespace as the result
    template.innerHTML = html;
    return template.content.firstChild;
}
const baseUrl = document.querySelector('base').href;

/*
    UserList FollowToggle functions
*/
var Users = document.querySelectorAll("a[data-joinId]");
var spinners = document.querySelectorAll('.spinner');

const actions = {
    Group: {
        isJoined: "Group/IsMember",
        toggle: "Group/ToggleJoin",
        joinText: "Join",
        splitText: "Leave"

    },
    Follow: {
        isJoined: `User/IsFollowing`,
        toggle: "User/ToggleFollow",
        joinText: "Follow",
        splitText: "Unfollow"
    }
};

Users.forEach(user => {
    setJoinText(user);
});

Users.forEach(user => { user.addEventListener('click', join); });

function join() {
    let set = this.dataset;
    let join = { ...set };
    let ele = this;
    this.parentElement.classList.add('spinner');
    this.textContent = '';
    removeButtonClasses(this);

    toggleJoin(join.userid, join.joinid, join.type)
        .then(result => {
            console.log(result);
            console.log("Update text");
            setJoinText(ele);
        });
}

function isJoined(userId, joinId, type) {
    return $.ajax({
        url: `${baseUrl}${actions[`${type}`].isJoined}/${userId}/${joinId}`,
        method: "GET"
    });
}

function toggleJoin(userId, joinId, type) {
    return $.ajax({
        url: `${baseUrl}${actions[`${type}`].toggle}/${userId}/${joinId}`,
        method: "POST"
    });
}

function setJoinText(element) {
    let set = element.dataset;
    let join = { ...set };
    removeButtonClasses(element);

    isJoined(join.userid, join.joinid, join.type)
        .then(result => {
            let text = result ? actions[`${join.type}`].splitText : actions[`${join.type}`].joinText;
            toggleSpinner(element.parentElement);
            element.textContent = text;
            element.classList.add('btn');
            if (result === true) {
                element.classList.add('btn-danger');
            }
            else {
                element.classList.add('btn-primary');
            }
        });
}

function toggleSpinner(spinner) {
    spinner.classList.toggle('spinner');
}

function removeButtonClasses(element) {
    element.classList.remove('btn');
    element.classList.remove('btn-primary');
    element.classList.remove('btn-danger');
}