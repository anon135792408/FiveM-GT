$(document).ready(function () {

    window.addEventListener('message', function (event) {
        var item = event.data;
        
        if (item.type == "InitCountdown") {
            startCountdown();
        } else if (item.type == "InitRaceIntro") {
            startRaceIntro();
        } else if (item.type == "PlayRandomSong") {
            playRandomSong();
        }

    });
})


