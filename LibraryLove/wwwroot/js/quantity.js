// Code to get the required input fields and submit the form

let selectedBookId;
let currentQuantity;
let quantityElement
let preLoanedBook = false; 


document.querySelectorAll('.addQuantityBtn').forEach(btn => {
    btn.addEventListener('click', event => {
        // set the bookId
        selectedBookId = btn.getAttribute('book-id');
        document.querySelector('.hiddenBookId').setAttribute('value', selectedBookId);

        // get the current quantity
        quantityElement = event.target.parentElement.previousElementSibling;
        currentQuantity = quantityElement.innerHTML;

        // see if the book is pre loaned only  or not
        if (currentQuantity == 0) {
            currentQuantity = 0;  // there are no books
            preLoanedBook = true; 
        }

        // set the current quantity in the form 
        document.querySelector('.hiddenQuantityBooks').setAttribute('value', currentQuantity);
    });
});

document.querySelector('.submit').addEventListener('click', event => {
    // Retrive the input that the user requested
    let addedQuantity = document.querySelector('.addedQuantity').value; 

    // Convert string types to integers
    currentQuantity = parseInt(currentQuantity);
    addedQuantity = parseInt(addedQuantity);

    // change the quantity - server
     document.querySelector('form').submit();

 
});





   