function GetFirstDate(year, month) {
    Date.prototype.addDays = function (n) {
        var time = this.getTime();
        var changedDate = new Date(time + (n * 24 * 60 * 60 * 1000));
        this.setTime(changedDate.getTime());
        return this;
    };

    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var dayName = '';
    var d = new Date(year, month - 1, 1)
    dayname = days[d.getDay()];
    index = days.indexOf(dayname)

    d.addDays(-index)
    console.log('first Date: ', d);
    return d;
}

function GetLastDate(year, month) {
    Date.prototype.addDays = function (n) {
        var time = this.getTime();
        var changedDate = new Date(time + (n * 24 * 60 * 60 * 1000));
        this.setTime(changedDate.getTime());
        return this;
    };

    var lastDay = new Date(year, month + 1, 0);

    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var dayName = '';
    var d = new Date(`${year}-${month}-${lastDay.getDate()}`)
    dayname = days[d.getDay()];

    index = days.indexOf(dayname)
    // console.log(index)

    d = d.addDays(6 - index)
    return d;
}

async function GetTask(year, month) {
    var url = `https://localhost:3000/TestDrives/GetList?year=${year}&month=${month}`;

    return await $.get(url, function (data, status) {
        return data;
    });
}

async function RenderCalendar(year, month) {
    Date.prototype.addDays = function (n) {
        this.setDate(currentDate.getDate() + n);
    };
    // TODO:
    // Return an HTML elemnet.Which is rendered dates from GetFirstDate() -> GetLastDate()
    var firstD = GetFirstDate(year, month);
    
    var currD = new Date(firstD.getFullYear(), firstD.getMonth(), firstD.getDate());
    // console.log(currD);
    var task = await GetTask(year, month);
    var matrix = new Array(5);

    console.log(task);

    for (var i = 0; i < matrix.length; i++) {
        matrix[i] = new Array(7);
        for (var j = 0; j < matrix[i].length; j++) {
            matrix[i][j] = new Date(currD.getFullYear(), currD.getMonth(), currD.getDate())
            currD.addDays(1);
        }
    }

    var txt = `<span class="day-name">Sun</span>
        <span class="day-name">Mon</span>
        <span class="day-name">Tue</span>
        <span class="day-name">Wed</span>
        <span class="day-name">Thu</span>
        <span class="day-name">Fri</span>
        <span class="day-name">Sat</span>`

    //console.log(matrix);
    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < matrix[i].length; j++) {
            var temp = matrix[i][j].getDate();
            var tmonth = matrix[i][j].getMonth();
            
            txt += `<div class="day ${tmonth != month - 1 ? 'day--disabled' : ''}">${temp}</div>`
        }
    }

    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < matrix[i].length; j++) {
            var temp = matrix[i][j];
            for (var q = 0; q < task.length; q++) {
                var tDate = task[q].bookDate && new Date(task[q].bookDate);

                if (tDate.getFullYear() === matrix[i][j].getFullYear()
                    && tDate.getMonth() === matrix[i][j].getMonth()
                    && tDate.getDate() === matrix[i][j].getDate()) {
                    // console.log(i, j);
                    txt += `
                    <section class="task task--warning" 
                        style="grid-column: ${j + 1} / span 1 !important; 
                        grid-row: ${i + 2} !important">
                        <a href="/TestDrives/Details/${task[q].driveId}" style="color: rgba(150, 150, 0)">
                            <b>${task[q].clientId}:</b> 
                            <small>${task[q].note}</small>
                        </a>
                    </section>`
                }
            }
        }
    }

    var elem = document.createElement('div');
    elem.classList.add('calendar');

    elem.innerHTML = txt;
    return elem;
}

