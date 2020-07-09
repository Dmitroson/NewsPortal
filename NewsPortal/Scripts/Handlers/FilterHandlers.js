let filterString = document.getElementById('filterString');
filterString = loadSessionData();

let filter = document.getElementById('filter');

filter.addEventListener('click', function () {
    let checkboxToday = document.getElementById('today');
    let checkboxYesterday = document.getElementById('yesterday');
    let checkboxWeek = document.getElementById('week');
    let checkboxAll = document.getElementById('all');

    if (checkboxToday.checked) {
        sessionStorage.setItem('today', '1');
    } else {
        sessionStorage.setItem('today', '0');
    }

    if (checkboxYesterday.checked) {
        sessionStorage.setItem('yesterday', '1');
    } else {
        sessionStorage.setItem('yesterday', '0');
    }

    if (checkboxWeek.checked) {
        sessionStorage.setItem('week', '1');
    } else {
        sessionStorage.setItem('week', '0');
    }

    if (checkboxAll.checked) {
        sessionStorage.setItem('all', '1');
    } else {
        sessionStorage.setItem('all', '0');
    }
});

function loadSessionData() {
    let filter = {
        today: Boolean(sessionStorage.getItem('today')),
        yesterday: Boolean(sessionStorage.getItem('yesterday')),
        week: Boolean(sessionStorage.getItem('week')),
        all: Boolean(sessionStorage.getItem('all')),
    };

    if (filter.today)
        filterString += filter.today + " ";

    if (filter.yesterday)
        filterString += filter.yesterday + " ";

    if (filter.week)
        filterString += filter.week + " ";

    if (filter.all)
        filterString += filter.all + " ";

    return filterString;
}



//checkboxYesterday.addEventListener('click', function () {
//    if (checkboxYesterday.checked) {
//        sessionStorage.setItem('yesterday', '1');
//    } else {
//        sessionStorage.setItem('yesterday', '0');
//    }
//});

//checkboxWeek.addEventListener('click', function () {
//    if (checkboxWeek.checked) {
//        sessionStorage.setItem('week', '1');
//    } else {
//        sessionStorage.setItem('week', '0');
//    }
//});

//checkboxAll.addEventListener('click', function () {
//    if (checkboxAll.checked) {
//        sessionStorage.setItem('all', '1');
//    } else {
//        sessionStorage.setItem('all', '0');
//    }
//});