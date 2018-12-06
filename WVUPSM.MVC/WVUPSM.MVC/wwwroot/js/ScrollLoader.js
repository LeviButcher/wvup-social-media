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

