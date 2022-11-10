function initilizeInactivityTimer(dotnetHelper, timeToLogout) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;
    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, timeToLogout);
    }
    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}