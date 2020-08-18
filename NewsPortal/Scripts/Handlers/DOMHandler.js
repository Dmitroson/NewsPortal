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

document.addEventListener("DOMContentLoaded", function () {
    let url = location.href.toString();
    if (url.includes('filterString'))
        showCheckedItems();

    let linkMoveToStart = document.querySelector('#paging-move-to-start');
    let linkMoveToEnd = document.querySelector('#paging-move-to-end');
    let linkMoveToPrevious = document.querySelector('#paging-move-to-previous');
    let linkMoveToNext = document.querySelector('#paging-move-to-next');

    if (linkMoveToStart)
        linkMoveToStart.innerHTML = '&laquo;';
    if (linkMoveToEnd)
        linkMoveToEnd.innerHTML = '&raquo;';
    if (linkMoveToPrevious)
        linkMoveToPrevious.innerHTML = '&lsaquo;';
    if (linkMoveToNext)
        linkMoveToNext.innerHTML = '&rsaquo;';
});