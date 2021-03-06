﻿let searchForm = document.getElementById('searchForm');

searchForm.addEventListener('submit', function () {
    saveCheckboxesToSessionData();
    let filterStringFromSessionData = loadSessionData();
    
    if (filterStringFromSessionData != '') {
        let filterInput = document.createElement('input');
        filterInput.type = 'hidden';
        filterInput.name = 'filterString';
        filterInput.value = filterStringFromSessionData;
        searchForm.appendChild(filterInput);
    }
});

let filter = document.getElementById('filter');

filter.addEventListener('click', function () {
    saveCheckboxesToSessionData();
    let params = document.getElementById('params');
    params.value = loadSessionData();
});


function saveCheckboxesToSessionData() {
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
}

function saveSorting() {
    let dateOrder = document.getElementById('date');
    let titleOrder = document.getElementById('title');
    let descriptionOrder = document.getElementById('description');

    dateOrder.addEventListener('click', function () {
        sessionStorage.setItem('sortOrder', 'date');
    });

    titleOrder.addEventListener('click', function () {
        sessionStorage.setItem('sortOrder', 'title');
    });

    descriptionOrder.addEventListener('click', function () {
        sessionStorage.setItem('sortOrder', 'description');
    });
}

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