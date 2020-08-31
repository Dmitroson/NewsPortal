let createButton = document.querySelector('#submitButton');

createButton.addEventListener('click', function (e) {
    let errorMessageForTitle = document.querySelector('#errorMessageForTitle');
    let title = document.querySelector("#title");
    if (title.value == "") {
        errorMessageForTitle.innerHTML = 'Title is required.';
        event.preventDefault();
    }

    let errorMessageForDescription = document.querySelector('#errorMessageForDescription');
    let editor = document.querySelector('iframe').contentWindow.document.body;

    if (editor.innerHTML == '<p><br></p>') {
        errorMessageForDescription.classList.remove('field-validation-valid');
        errorMessageForDescription.classList.add('field-validation-error');
        errorMessageForDescription.innerHTML = 'Description is required.';
        event.preventDefault();
    }
    else {
        errorMessageForDescription.classList.remove('field-validation-error');
        errorMessageForDescription.classList.add('field-validation-valid');
        errorMessageForDescription.innerHTML = '';
    }


    let errorMessageForImage = document.querySelector('#errorMessageForImage');
    let uploadImage = document.querySelector('#uploadImage');

    if (!uploadImage.classList.contains('valid')) {
        errorMessageForImage.innerHTML = 'Image is required.';
        event.preventDefault();
    }
    else {
        errorMessageForImage.innerHTML = '';
    }
});