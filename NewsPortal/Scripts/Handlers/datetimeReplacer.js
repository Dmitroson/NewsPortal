let pubDateInput = document.getElementById('pubDateInput');
let pubDate = new Date(pubDateInput.value);

if (pubDateInput.value) {
    pubDateInput.value = convertToLocaleDateTime();
}

function convertToLocaleDateTime() {
    let offset = getOffset();
    pubDate.setHours(pubDate.getHours() + offset);
    let localTime = addZero(pubDate.getFullYear()) + '-'
        + addZero(pubDate.getMonth()+1) + '-'
        + addZero(pubDate.getDate()) + 'T'
        + addZero(pubDate.getHours()) + ':'
        + addZero(pubDate.getMinutes());
    return localTime.substr(0, 16);
}

function addZero(num) {
    var str = num.toString();
    return str.length == 1 ? "0" + str : str;
}

let submitButton = document.getElementById('submitButton');

submitButton.addEventListener('click', function () {
    let pubDateInput = document.getElementById('pubDateInput');
    if (pubDateInput.value) {
        let utcDateTime = new Date(pubDateInput.value);
        pubDateInput.value = utcDateTime.toISOString().substr(0, 16);
    }
});

function getOffset() {
    let x = new Date();
    let offset = -x.getTimezoneOffset() / 60;
    return offset;
}

