$(document).ready(function () {

    window.addEventListener('message', function (event) {
        var item = event.data;

        if (item.InitOpeningMovie == true) {
            $('#wrap').show();
        } else {

        }
    });
})


