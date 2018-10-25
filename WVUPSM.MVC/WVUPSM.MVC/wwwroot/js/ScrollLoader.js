//TODO: Make Scroll loader responsible for skip take
/*
 * element => element to add new content to
 * render => function that can take in json and output HTML elements
 * apiCall => string of the api to call for data
 * scrollTop => bool: true if we should load content based on top scrolled
 * */
function scrollLoader(element, render, apiURL, skipStart, take, scrollTop,) {
    let skip = skipStart;

    element.addEventListener('scroll', (event) => {
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
                        element.appendChild(htmlToElement(html));
                    });
                })
                .then(() => skip = skip + take);
        }
    });
}

async function CallApi(apiURL, skip, take) {
    return $.ajax({
        url: `${baseUrl}${apiURL}?skip=${skip}&take=${take}`,
        method: "GET"
    }).done((data) => data);
}