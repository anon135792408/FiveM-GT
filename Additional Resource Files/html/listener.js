$(document).ready(function () {

    window.addEventListener('message', function (event) {
        var item = event.data;
        
        if (item.type == "InitCountdown") {
            startCountdown();
        } else if (item.type == "InitRaceIntro") {
            startRaceIntro();
        } else if (item.type == "PlayRandomSong") {
            playRandomSong();
        } else if (item.type == "SetMusicVolume") {
            setMusicVolume(item.MusicVolume);
        } else if (item.type == "SetSfxVolume") {
            setSfxVolume(item.SfxVolume);
        } else if (item.type == "SetLaps") {
            setRaceLaps(item.Laps);
        } else if (item.type == "SetRaceCurrentLap") {
            setRaceCurrentLap(item.Lap);
        } else if (item.type == "StopAllMusic") {
            stopAllMusic();
        } else if (item.type == "PlayFinishingSong") {
            playFinishingSong();
        } else if (item.type == "ShowSoundOptions") {
            $("#soundOptionsModal").show();
        }

    });
})


