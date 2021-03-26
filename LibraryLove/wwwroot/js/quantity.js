// Code to get the required input fields and submit the form

let selectedBookId;
let currentQuantity;

document.querySelectorAll('.addQuantityBtn').forEach(btn => {
    btn.addEventListener('click', event => {
        // set the bookId
        selectedBookId = btn.getAttribute('book-id');
        document.querySelector('.hiddenBookId').setAttribute('value', selectedBookId);

        // get the current quantity
        let quantityElement = event.target.parentElement.previousElementSibling;
        currentQuantity = quantityElement.innerHTML;

        if (currentQuantity == "Pre Loan") {
            currentQuantity = 0;
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
           // change the quantity - total has not reached 20
            document.querySelector('form').submit();

        }
    }

  


});





   