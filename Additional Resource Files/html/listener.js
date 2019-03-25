$(document).ready(function () {

    window.addEventListener('message', function (event) {
        var item = event.data;
        
        if (item.type == "InitCountdown") {
            startCountdown();
        }

    });
})


