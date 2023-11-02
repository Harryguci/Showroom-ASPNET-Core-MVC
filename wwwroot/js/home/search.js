function handleChange(event) {
    var value = event.target.value;

    if (value)
        document.querySelector('#layout-search-btn').removeAttribute('disabled')
    else
        document.querySelector('#layout-search-btn').setAttribute('disabled', 'true')


    var maxW = Math.min(window.screen.width, 500);
    event.target.style.width = maxW + 'px';

    var url = `https://localhost:3000/Home/SearchApi?q=${value}`;
    $.get(url, function (response, status) {
        response = JSON.parse(response);

        var html = `<ul class="list">`;
        var max_count = 3;

        for (var x of response.customers) {
            if (x) {
                html += `<li class="list-item"><h4><a href="/Customers/Details/${x.ClientId}">${x.ClientId}</a></h4><p>${x.Firstname} ${x.Lastname}</p></li>`
                max_count--;
            }
            if (max_count <= 0) break
        }

        max_count = 3;

        for (var x of response.products) {
            if (x) {
                html += `<li class="list-item"><h4><a href="/Products/Details/${x.Serial}">${x.Serial}</a></h4><p>${x.ProductName}</p></li>`
                max_count--;
            }
            if (max_count < 0) break
        }

        max_count = 3;

        for (var x of response.employees) {
            if (x) {
                html += `<li class="list-item"><h4><a href="/Employees/Details/${x.EmployeeId}">${x.EmployeeId}</a></h4><p>${x.Firstname} ${x.Lastname}</p></li>`
                max_count--;
            }
            if (max_count < 0) break
        }
        html += `<ul>`;

        var resElement = document.querySelector('#search-result')
        if (resElement.classList.contains('d-none')) {
            resElement.classList.remove('d-none');
        }
        // console.log(html)
        resElement.innerHTML = html;
    });
}

function handleBlurSearchInput(event) {
    event.target.style.width = '100%';
    document.querySelector('#search-result').innerHTML = "";
}