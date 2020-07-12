let searchButton = document.getElementById('searchButton');
let searchString = document.getElementById('searchString');

searchButton.addEventListener('click', function () {
    sessionStorage.setItem('searchString', searchString.value);
});