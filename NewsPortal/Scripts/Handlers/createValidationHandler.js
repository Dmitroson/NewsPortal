let createButton = document.querySelector('#submitButton');

createButton.addEventListener('click', function () {
    let errorMessageForDescription = document.querySelector('#errorMessageForDescription');
    let editor = document.querySelector('iframe').contentWindow.document.body;

    if (editor.innerHTML == '<p><br></p>') {
        errorMessageForDescription.classList.remove('field-validation-valid');
        errorMessageForDescription.classList.add('field-validation-error');
        errorMessageForDescription.innerHTML = 'Description is required.';
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
    }
    else {
        errorMessageForImage.innerHTML = '';
    }
});