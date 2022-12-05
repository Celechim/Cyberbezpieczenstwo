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

function googleRecaptcha(dotNetObject, selector, sitekeyValue) {
    return grecaptcha.render(selector, {
        "sitekey": "6LfToQAjAAAAALLgf75xT6ic4_ZueiiEkIlO6C4U",
        "callback": (response) => { dotNetObject.invokeMethodAsync("CallbackOnSuccess", response); },
        "expired-callback": () => { dotNetObject.invokeMethodAsync("CallbackOnExpired", response); }
    });
}

function getResponse(response) {
    return grecaptcha.getResponse(response);
}
async function GetFileToDownload(fileName, contentStreamReference)
    {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    }
