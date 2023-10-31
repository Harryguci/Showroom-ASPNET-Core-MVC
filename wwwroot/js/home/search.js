function handleChange(event) {
    var value = event.target.value;
    // console.log(value);
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
                html += `<li class="list-item"><h4><a href="/Products/Details/${x.ProductId}">${x.ProductId}</a></h4><p>${x.ProductName}</p></li>`
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

        resElement.innerHTML = html;
    });
}