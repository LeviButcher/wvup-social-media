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