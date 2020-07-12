function showCheckedItems() {
    let today = sessionStorage.getItem('today');
    let yesterday = sessionStorage.getItem('yesterday');
    let week = sessionStorage.getItem('week');
    let all = sessionStorage.getItem('all');

    let checkboxToday = document.getElementById('today');
    let checkboxYesterday = document.getElementById('yesterday');
    let checkboxWeek = document.getElementById('week');
    let checkboxAll = document.getElementById('all');

    if (today == '1')
        checkboxToday.checked = true;

    if (yesterday == '1')
        checkboxYesterday.checked = true;

    if (week == '1')
        checkboxWeek.checked = true;

    if (all == '1')
        checkboxAll.checked = true;
}

function isFilterActive() {
    let checkboxes = document.getElementsByTagName('input');
    for (let i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            return true;
        }
        return false;
    }
}

function fillSearchString() {
    let searchField = document.getElementById('searchString');
    let searchString = sessionStorage.getItem('searchString');
    searchField.value = searchString;
}

document.addEventListener("DOMContentLoaded", function () {
    showCheckedItems();
    if (isFilterActive()) {
        fillSearchString();
    }
});