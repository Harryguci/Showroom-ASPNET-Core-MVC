async function HandleToggleNotiMenu(event, id) {
    if (!document.getElementsByClassName('notification-menu')[0].classList.contains('d-none')) {
        document.getElementsByClassName('notification-menu')[0].classList.add('d-none');
        return;
    }
    document.getElementsByClassName('notification-menu')[0].classList.remove('d-none');

    var url = `/api/TasksApi/Employees/${id}`;
    var tasksList = await fetch(url)
        .then(response => response.json());
    var listElem = document.createElement('ul');

    //console.log(tasksList);
    for (var x of tasksList) {
        var li = document.createElement('li');
        var title = document.createElement('p');
        var dateline = document.createElement('small');
        title.innerHTML = x.content;

        dateline.innerHTML = new Date(x.dateline).toLocaleDateString("en-US", {
            year: "numeric",
            month: "2-digit",
            day: "2-digit"
        });

        li.appendChild(title);
        li.appendChild(dateline);
        listElem.appendChild(li);
    }

    var elem = document.createElement('a');
    elem.style.textAlign = 'center';
    elem.style.display = 'block';
    elem.style.padding = '3px';
    elem.innerHTML = 'Xem tất cả';
    elem.setAttribute('href', '/Accounts/Person');

    var li = document.createElement('li');
    li.appendChild(elem);

    listElem.appendChild(li)

    $('.notification-menu').html(listElem);
}