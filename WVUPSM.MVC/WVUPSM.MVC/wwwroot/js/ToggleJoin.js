var baseUrl = document.querySelector('base').href;

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