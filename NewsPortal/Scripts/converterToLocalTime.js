let elements = document.getElementsByClassName('pubDate');

for (let i = 0; i < elements.length; i++) {
    let serverTime = elements[i].innerText;
    let culture = getCulture(serverTime);

    let localTime = getLocalTime(serverTime, culture);
    elements[i].innerText = localTime;
}

function getOffset() {
    let x = new Date();
    let offset = -x.getTimezoneOffset() / 60;
    return offset;
}

function parseTime(time) {
    let culture = getCulture(time);
    let timeArray;
    if (culture == 'ru') {
        time = time.replace(' ', '.');
        time = time.replace(':', '.');
        time = time.replace(':', '.');
        timeArray = time.split('.');
    }
    else {
        time = time.replace(' ', '/');
        time = time.replace(' ', '/');
        time = time.replace(':', '/');
        time = time.replace(':', '/');
        timeArray = time.split('/');
    }

    let timeParsed = {};

    if (culture == 'ru') {
        timeParsed = {
            year: timeArray[2],
            month: timeArray[1],
            day: timeArray[0],
            hours: timeArray[3],
            minutes: timeArray[4]
        };
    }
    else {
        timeParsed = {
            year: timeArray[2],
            month: timeArray[0],
            day: timeArray[1],
            hours: timeArray[3],
            minutes: timeArray[4],
            meridiem: timeArray[6]
        };

        if (timeParsed.meridiem == 'AM' && timeParsed.hours == 12) {
            timeParsed.hours -= 12;
        }

        if (timeParsed.meridiem == 'PM' && timeParsed.hours != 12) {
            timeParsed.hours = (+timeParsed.hours + 12).toString();
        }
    }
    return timeParsed;
}

function getLocalTime(time, culture = 'ru') {
    let offset = getOffset();
    
    let localTime = parseTime(time);
    let dateTime = new Date(localTime.year, localTime.month, localTime.day, localTime.hours, localTime.minutes);
    dateTime.setHours(dateTime.getHours() + offset);

    let year = dateTime.getFullYear().toString();
    let month = dateTime.getMonth().toString();
    let day = dateTime.getDate().toString();
    let hours = dateTime.getHours().toString();
    let minutes = dateTime.getMinutes().toString();

    if (month.length < 2) {
        month = '0' + month;
    }
    if (day.length < 2) {
        day = '0' + day;
    }
    if (hours.length < 2) {
        hours = '0' + hours;
    }
    if (minutes.length < 2) {
        minutes = '0' + minutes;
    }

    let localTimeString = '';
    if (culture == 'ru') {
        localTimeString += day + '.'
            + month + '.'
            + year + ' '
            + hours + ':'
            + minutes;
    }
    else {
        if (Number(hours) > 12) {
            hours -= 12;
            localTime.meridiem = 'PM';
        }

        localTimeString += month + '/'
            + day + '/'
            + year + ' '
            + hours + ':'
            + minutes + ' '
            + localTime.meridiem;
    }

    return localTimeString;
}

function getCulture(time) {
    let culture;
    if (time.includes('.')) {
        culture = 'ru';

    }
    else if (time.includes('/')) {
        culture = 'en';
    }
    return culture;
}