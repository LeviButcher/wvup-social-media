function toggleCommentForm() {
    let post = event.srcElement.dataset.post;
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
 * apiCall => string of the api to call for data
 * scrollTop => bool: true if we should load content based on top scrolled
 * callback => optional function to execute after load
 * */
function scrollLoader(scrollElement, insertIntoElement, apiURL, skipStart, take, scrollTop, callback) {
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
                .then(htmlData => {
                    //decode HTML to HTMLCollection, spread that into a array to give foreach
                    //Then add each element into desired element
                    [...decodeHtml(htmlData)].forEach(child => {
                        insertIntoElement.appendChild(child);
                    });
                })
                .then(() => { skip = skip + take; })
                .then(() => callback !== undefined ? callback(): "");
        }
    });
}

async function CallApi(apiURL, skip, take) {
    return $.ajax({
        url: `${baseUrl}${apiURL}?skip=${skip}&take=${take}`,
        method: "GET",
        dataType: "html"
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
 */
function decodeHtml(html) {
    var txt = document.createElement("div");
    txt.innerHTML = html;
    return txt.children;
}
const baseUrl = document.querySelector('base').href;

/*
    UserList FollowToggle functions
*/
let Users = document.querySelectorAll("a[data-joinId]");

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

function updateUserList() {
    Users = document.querySelectorAll("a[data-joinId]");
    Users.forEach(user => {
        setJoinText(user);
    });

    Users.forEach(user => { user.addEventListener('click', join); });
}

updateUserList();

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
            setJoinText(ele, true);
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

function setJoinText(element, change = false) {
    let set = element.dataset;
    let join = { ...set };
    if(change) removeButtonClasses(element);

    isJoined(join.userid, join.joinid, join.type)
        .then(result => {
            let text = result ? actions[`${join.type}`].splitText : actions[`${join.type}`].joinText;
            removeSpinner(element.parentElement);
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

function removeSpinner(spinner) {
    spinner.classList.remove('spinner');
}

function removeButtonClasses(element) {
    element.classList.remove('btn');
    element.classList.remove('btn-primary');
    element.classList.remove('btn-danger');
}