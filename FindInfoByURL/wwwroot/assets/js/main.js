$("document").ready(() => {
    const host = "http://localhost:5000/";
    const form = $("#url-form");
    const textAreaOfUrls = $("#urls");
    const table = $("#tableOfResults");

    $(form).submit((event) => {
        event.preventDefault();

        $(table).find("tr[data-index]").remove();

        const textAreaContent = textAreaOfUrls.val();
        const urls = textAreaContent.split("\n").filter((url) => url != '' && !/\s+/.test(url));

        let indexOfRow = 0;
        for (const url of urls) {

            InitializeRowInTable(indexOfRow);
            SendUrl(url, indexOfRow);

            indexOfRow++;
        }
    });

    function SendUrl(url, indexOfRow) {
        let query = `?url=${encodeURI(url)}`;

        fetch(`${host}api/home${query}`).then(async (response) => {
            let body = await response.json();

            let row = $(table).find(`tr[data-index=${indexOfRow}]`)[0];
            let tdTitle = $(row).find(`td[data-type="title"]`)[0];
            let tdStatusCode = $(row).find(`td[data-type="statusCode"]`)[0];
            
            $(tdTitle).html(body.title);
            $(tdStatusCode).html(body.statusCode);
        });
    }

    function InitializeRowInTable(indexOfRow) {
        let tr = document.createElement("tr");
        $(tr).attr("data-index", indexOfRow);

        const spinner = "<div class=\"spinner\"><\/div>";

        let tdTitle = document.createElement("td");
        $(tdTitle).attr("data-type", "title");
        $(tdTitle).html(spinner);

        let tdStatusCode = document.createElement("td");
        $(tdStatusCode).attr("data-type", "statusCode");
        $(tdStatusCode).html(spinner);

        $(tr).append(tdTitle);
        $(tr).append(tdStatusCode);

        $(table).append(tr);
    }

    const statisticsBtn = $("#statistics-tab");
    const statisticsTable = $("#statisticsTable");

    $(statisticsBtn).click(() => {
        fetch(`${host}api/home/requests`)
            .then(async (response) => {
                let requests = await response.json();

                InitializeStatisticsTable(requests);
            });
    });

    function InitializeStatisticsTable(requests) {
        $(statisticsTable).find("tr[data-index]").remove();

        if (requests.length !== 0) {
            let indexOfRow = 0;

            for (const request of requests) {
                let tr = document.createElement("tr");
                $(tr).attr("data-index", indexOfRow);

                for (const prop in request) {
                    if (prop === "Id" || prop === "id")
                        continue;

                    let td = document.createElement("td");
                    $(td).attr("data-type", prop.toString());

                    if (prop === "URL" || prop === "url") {
                        let a = document.createElement("a");

                        $(a).attr("href", "#");
                        $(a).attr("id", indexOfRow + '_' + "url_id");

                        $(a).click(handlingUrlClick);
                        $(a).text(request[prop]);

                        $(td).attr("data-url", request[prop]);
                        $(td).html(a);
                    } else {
                        $(td).html(request[prop]);
                    }

                    $(tr).append(td);
                }

                $(statisticsTable).append(tr);
                indexOfRow++;
            }
        } else {
            let tr = document.createElement("tr");
            let td = document.createElement("td");

            $(tr).attr("data-index", -1);
            $(td).attr("colspan", 4);
            $(td).html("<b>You haven't done yet any requests.</b>");
            $(tr).append(td);
            $(statisticsTable).append(tr);
        }
    }

    function handlingUrlClick(event) {
        event.preventDefault();

        const relatedRows = $(statisticsTable).find(`td[data-url='${$(event.target).text()}']`).parent();
        console.log(relatedRows);

        const dates = relatedRows
            .find("td[data-type='dateTime']")
            .toArray()
            .map(td => $(td).text());

    }
});