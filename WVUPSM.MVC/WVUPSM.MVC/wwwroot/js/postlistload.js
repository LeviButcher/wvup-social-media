/*
 * Loads a User Following Posts on scroll
 */

const postContainer = document.querySelector(".post-container");
const baseTake = 10;
let skip = baseTake;
let take = baseTake;

$(window).scroll(() => {
    //Stack Overflow - https://stackoverflow.com/questions/14035180/jquery-load-more-data-on-scroll
    //console.groupCollapsed("scroll");
    //console.log("window-scroll: " + $(window).scrollTop());
    //console.log("window-height: " + $(window).height());
    //console.log("document-height: " + $(document).height());
    //console.groupEnd("scroll");
    if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
        console.log("triggered");
        let userId = postContainer.dataset.userid;
        let action = postContainer.dataset.action;
        AddPostData(userId, action);
    }
});


function AddPostData(userId, action) {
    GetPostData(userId, action, skip, take)
        .then(data => {
            data.map(datum => {
                let postArticle = document.createElement('article');
                postArticle.classList.add('post');
                let postHeader = document.createElement('header');
                postHeader.classList.add('post-header');
                let postContent = document.createElement('div');
                postContent.classList.add('post-content');
                postHeader.innerHTML = `<h3><a asp-controller="User" asp-action="Index" asp-route-userId="${datum.userId}"> ${datum.userName} </a></h3>
                                            <h4><a asp-controller="Post" asp-action="Index" asp-route-postId="${datum.postId}"> ${datum.dateCreated} </a></h4>
                                        `.trim();
                postContent.innerHTML = `<p>
                                 ${datum.text}
                               </p>`.trim();

                postArticle.appendChild(postHeader);
                postArticle.appendChild(postContent);
                postContainer.appendChild(postArticle);
            });
        }).then(() => {
            skip += baseTake;
            take += baseTake;
            console.log({ skip, take });
        });
}

async function GetPostData(userId, action, skip, take) {
    return $.ajax({
        url: `${baseUrl}/${action}/${userId}?skip=${skip}&take=${take}`,
        method: "GET"
    }).done((data) => data);
}

