async function HandleSubmitUpdateTask(event) {
    event.preventDefault();

    var data = {
        Id: document.querySelector('.update-task-container input[name="Id"]').value,
        Content: document.querySelector('.update-task-container input[name="Content"]').value,
        Result: document.querySelector('.update-task-container input[name="Result"]').value
    };

    await fetch(`/api/TasksApi/${data.Id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    // Remove the form:
    $('.update-task-container').parent().remove();
    return location.reload();
}

async function HandleChangeEmployeeId(event) {
    console.log(event.target.value);

    var employeeId = event.target.value;
    await $.ajax({
        url: `/Employees/Search?employeeId=${employeeId}`,
        type: 'GET',
        dataType: 'json', // added data type
        success: function (res) {
            // console.log(res);
            // alert(res);
            console.log(res[0].Firstname)
            // `<select class="form-control"><option>option</option></select>`
            var html = `<ul class="list-group">`;
            for (var i in res) {
                html += `<li class="list-group-item" value="${res[i].EmployeeId}">${res[i].Firstname} ${res[i].Lastname}</li>`
            }
            html += '</ul>';

            // $('#ClientId').val(res[0].ClientId)
            document.querySelector('#employee-list').classList.remove('d-none');
            document.querySelector('#employee-list').innerHTML = html
        }
    });

    $('#employee-list li').on('click', function () {
        $('#EmployeeId').val($(this).attr('value'))
        $('#employee-list li').removeClass('active')
        $(this).addClass('active');
    })
}

async function HandleSubmitManagerTask(event) {
    event.preventDefault();

    if (confirm("Bạn chắc chắn muốn gửi?")) {
        var data = {
            EmployeeId: document.getElementById('EmployeeId').value,
            Content: document.getElementById('Content').value,
            Dateline: document.getElementById('Dateline').value
        };

        if (data.EmployeeId == '')
            return alert('Bạn phải điền mã nhân viên.');

        if (data.Content == '')
            return alert('Bạn phải điền nội dung công việc.');

        if (data.Dateline == '')
            return alert('Bạn phải chọn kỳ hạn cho công việc.');

        var url = '/api/TasksApi/'

        await fetch(url, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },

            //make sure to serialize your JSON body
            body: JSON.stringify(data)
        })
            .then((response) => {
                alert('Giao việc thành công!!');

                document.getElementById('EmployeeId').value = '';
                document.getElementById('Content').value = '';
                document.getElementById('Dateline').value = '';
                document.getElementById('employee-list').classList.add('d-none')
                console.log(response);
            });
    }
}