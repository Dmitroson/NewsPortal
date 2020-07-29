document.addEventListener("DOMContentLoaded", function () {
    let url = location.href.toString().split('/');
    for (let i = 0; i < url.length; i++) {
        if (url[i] == "en") {
            language = "en";
            break;
        }
        else {
            language = "ru";
        }
    }
    let checkedLang = document.getElementById(language);
    checkedLang.classList.toggle("selected");
});