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
});