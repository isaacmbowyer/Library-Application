// get which theme was selected in local storage 
let theme = localStorage.getItem('theme');
if (theme == null) {
    setTheme('light');
}
else if (theme == 'light') {
    setTheme('light');
}
else {
    setTheme('dark')
}

// set the new theme
function setTheme(mode) {
    if (mode == 'light') {
        document.getElementById('styles').href = "/css/defaultstyles.css";
    }
    else {
        document.getElementById('styles').href = "/css/darkstyles.css";
    }
    // save new theme
    localStorage.setItem('theme', mode);
}


// if the checkbox was ticked or unticked, change the theme 
let checkbox = document.getElementById('checkbox');
if (checkbox != null) {
    checkbox.addEventListener('change', event => {
        if (checkbox.checked) {
            setTheme('dark');
        } else {
            setTheme('light');
        }
    });
    if (theme == 'dark') {
        checkbox.checked = true;
    }
}
