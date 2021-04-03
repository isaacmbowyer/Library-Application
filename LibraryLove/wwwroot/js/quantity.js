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

    let errorMessage = document.querySelector('#error');

    // Check if the current quantity is 20 or more
    if (currentQuantity == 20) {
        errorMessage.innerHTML = "This Book Collection has reached max quantity"
    }
    else {
        // Check if the total exceeds 20
        let total = currentQuantity + addedQuantity;

        if (total > 20) {
            // Show error
            errorMessage.innerHTML = "Quantity of a Book Collection cannot exceed 20"
        }
        else {
            if (preLoanedBook) {
                // if the book is PreLoaned Only, we need to tell the server -> set current quantity to null
                //document.querySelector('.hiddenQuantityBooks').setAttribute('value', null);
            }

            // change the quantity - total has not reached 20
            document.querySelector('form').submit();

        }
    }

  


});





   