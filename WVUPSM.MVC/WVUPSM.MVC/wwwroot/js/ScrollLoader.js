
/*
 * element => element to add new content to
 * render => function that can take in json and output HTML elements
 * apiCall => string of the api to call for data
 * scrollTop => bool: true if we should load content based on top scrolled
 * */
function scrollLoader(element, render, apiCall, scrollTop) {
    console.dir(element);
    element.addEventListener('scroll', (event) => {
        let ele = event.target;
        if (ele.scrollHeight - ele.scrollTop === ele.clientHeight) {
            console.log('scrolled');
        }
    });
}



async function CallApi(apiCall) {
    return $.ajax({
        url: `${baseUrl}/${apiCall}`,
        method: "GET"
    }).done((data) => data);
}