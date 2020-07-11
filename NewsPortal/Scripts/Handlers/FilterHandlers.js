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

    let filterString = document.getElementById('filterString');
    filterString.value = loadSessionData();
});

function loadSessionData() {
    let today = sessionStorage.getItem('today');
    let yesterday = sessionStorage.getItem('yesterday');
    let week = sessionStorage.getItem('week');
    let all = sessionStorage.getItem('all');
    let filterString = '';

    if (today == '1')
        filterString += 'today ';

    if (yesterday == '1')
        filterString += 'yesterday ';

    if (week == '1')
        filterString += 'week ';

    if (all == '1')
        filterString += 'all ';

    return filterString;
}