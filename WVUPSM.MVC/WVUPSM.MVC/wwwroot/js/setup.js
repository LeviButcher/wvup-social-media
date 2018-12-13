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