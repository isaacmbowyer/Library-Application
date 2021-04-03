let selectedUserId;

document.querySelectorAll('.deleteBtn').forEach(btn => {
    btn.addEventListener('click', event => {
        // set the user id to pass on for handler
        selectedUserId = btn.getAttribute('user-id');
        document.querySelector('.hiddenUserId').setAttribute('value', selectedUserId);

    });
});
