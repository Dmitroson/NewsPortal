let pubDateInput = document.getElementById('pubDateInput');
let pubDateString = pubDateInput.value;

if (pubDateInput.value) {
    let localDateTime = convertToLocaleDateTime(pubDateString);
    pubDateInput.value = localDateTime;
}

function getOffset() {
    let x = new Date();
    let offset = -x.getTimezoneOffset() / 60;
    return offset;
}

function convertToLocaleDateTime(timeString) {
    let dateTime = parseDateTimeString(timeString);
    let timeArr = dateTime.time.split(':');
    let time = {
        hours: timeArr[0],
        minutes: timeArr[1]
    }
    let offset = getOffset();
    time.hours = (+time.hours + offset).toString();
    dateTime.time = time.hours + ':' + time.minutes;

    let localDateTime = dateTime.date + 'T' + dateTime.time;
    return localDateTime;
}

function parseDateTimeString(string) {
    let dateTimeArray = string.split('T');
    let dateTime = {
        date: dateTimeArray[0],
        time: dateTimeArray[1]
    }

    return dateTime;
}


function convertToUtcDateTime(timeString) {
    let dateTime = parseDateTimeString(timeString);
    let timeArr = dateTime.time.split(':');
    let time = {
        hours: timeArr[0],
        minutes: timeArr[1]
    }
    let offset = getOffset();
    time.hours = (time.hours - offset).toString();
    dateTime.time = time.hours + ':' + time.minutes;

    let localDateTime = dateTime.date + 'T' + dateTime.time;
    return localDateTime;
}

let submitButton = document.getElementById('submitButton');

submitButton.addEventListener('click', function () {
    let pubDateInput = document.getElementById('pubDateInput');
    if (pubDateInput.value) {
        let utcDateTime = convertToUtcDateTime(pubDateInput.value);
        pubDateInput.value = utcDateTime;
    }
});