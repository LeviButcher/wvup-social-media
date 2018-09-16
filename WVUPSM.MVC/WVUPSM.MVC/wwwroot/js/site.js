﻿var baseUrl = document.querySelector('base').href;

/*
    UserList FollowToggle functions
*/
var Users = document.querySelectorAll("a[data-userId]");
var spinners = document.querySelectorAll('.spinner');

Users.forEach(user => {
    setFollowingText(user);
});

Users.forEach(user => user.addEventListener('click', toggleFollow));

function toggleFollow() {
    let set = this.dataset;
    let follow = { ...set };
    console.log(follow);
    this.parentElement.classList.add('spinner');
    this.textContent = '';
    this.classList.remove('btn');
    this.classList.remove('btn-primary');

    $.ajax({
        url: `${baseUrl}/User/ToggleFollow/${follow.userid}/${follow.followid}`,
        method: "POST"
    }).done(() => {
        console.log('ToggleFollow successful');
        setFollowingText(this);
        console.dir(this);
    });
}

function setFollowingText(element) {
    console.dir(element);
    let set = element.dataset;
    let follow = { ...set };
    element.classList.remove('btn');
    element.classList.remove('btn-primary');

    isFollowing(follow.userid, follow.followid)
        .then(result => {
            console.log("Within async isFollowing " + result);
            let text = result ? "Following" : "Follow";
            toggleSpinner(element.parentElement);
            element.textContent = text;
            element.classList.add('btn');
            element.classList.add('btn-primary');
        });
}

async function isFollowing(userId, followId) {
    return $.ajax({
        url: `${baseUrl}/User/IsFollowing/${userId}/${followId}`,
        method: "GET",
        success: function (data) {
            console.log("success");
            console.log(data);
            return data;
        },
        error: (data) => {
            console.log("error");
            console.log(data);
        }
    });
}

function toggleSpinner(spinner) {
    spinner.classList.toggle('spinner');
}

/*
    Drawer Button Functionality
*/

var button = document.querySelector("#drawer-button");
var mainContent = document.querySelector("main");
var drawer = document.querySelector("#drawer");
var footer = document.querySelector(".site-footer");

button.addEventListener('click', toggleDrawer);

function toggleDrawer() {
    drawer.classList.toggle("primary-nav-drawer-active");
    mainContent.classList.toggle("active-drawer");
    footer.classList.toggle('site-footer-active');
}

