// Code to display the filename to the user

const fileElement = document.querySelector('.custom-file-input');
const fileLabel = document.querySelector('.custom-file-label');
const imgElement = document.querySelector('.book-image');

fileElement.addEventListener('change', (event) => {
    // get the fileName
    let fileName = fileElement.value.split('\\')[2];
    fileLabel.innerHTML = fileName;

});


