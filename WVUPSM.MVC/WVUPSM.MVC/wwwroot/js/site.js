var baseUrl = document.querySelector('base').href;

/*
    UserList FollowToggle functions
*/
var Users = document.querySelectorAll("[data-userId]");

Users.forEach(user => {
    setFollowingText(user);
});

Users.forEach(user => user.addEventListener('click', toggleFollow));

function toggleFollow() {
    let set = this.dataset;
    let follow = { ...set };
    console.log(follow);

    $.ajax({
        url: `${baseUrl}/User/ToggleFollow/${follow.userid}/${follow.followid}`,
        method: "POST"
    }).done(() => {
        console.log('successful');
        setFollowingText(this);
    });
}

function setFollowingText(element) {
    console.dir(element);
    let set = element.dataset;
    let follow = { ...set };

    isFollowing(follow.userid, follow.followid)
        .then(result => {
            console.log("Within async isFollowing " + result);
            let text = result ? "Following" : "Follow";
            element.textContent = text;
        });
}

async function isFollowing(userId, followId) {
    let isFollowing;

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