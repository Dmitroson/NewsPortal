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
            hour: timeArray[3],
            minute: timeArray[4]
        };
    }
    else {
        timeParsed = {
            year: timeArray[2],
            month: timeArray[0],
            day: timeArray[1],
            hour: timeArray[3],
            minute: timeArray[4],
            meridiem: timeArray[6]
        };

        if (timeParsed.meridiem == 'AM' && timeParsed.hour == 12) {
            timeParsed.hour -= 12;
        }

        if (timeParsed.meridiem == 'PM' && timeParsed.hour != 12) {
            timeParsed.hour = (+timeParsed.hour + 12).toString();
        }
    }
    return timeParsed;
}

function getLocalTime(time, culture = 'ru') {
    let offset = getOffset();
    
    let localTime = parseTime(time);
    localTime.hour = (+localTime.hour + offset).toString();


    let localTimeString = '';
    if (culture == 'ru') {
        localTimeString += localTime.day + '.'
            + localTime.month + '.'
            + localTime.year + ' '
            + localTime.hour + ':'
            + localTime.minute;
    }
    else {
        if (localTime.hour > 12) {
            localTime.hour -= 12;
            localTime.meridiem = 'PM';
        }

        localTimeString += localTime.month + '/'
            + localTime.day + '/'
            + localTime.year + ' '
            + localTime.hour + ':'
            + localTime.minute + ' '
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