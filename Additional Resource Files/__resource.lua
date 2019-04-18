resource_manifest_version '05cfa83c-a124-4cfa-a768-c24a5811d8f9'

resource_type 'gametype' { name = 'FiveM-GT' }

ui_page('html/countdown.html')

client_script {
	"FiveM-GT-Client.net.dll",
}

server_script {
	"FiveM-GT-Server.net.dll",
	"Newtonsoft.Json.dll"
}

files {
    'html/countdown.html',
    'html/countdown-style.css',
	'html/listener.js',
	'html/countdown.mp3',
	'html/intro.mp3',
	'html/finish.mp3',
	'html/font.ttf',
	'html/music/aisha.mp3',
	'html/music/justaday.mp3',
	'html/music/kickstartmyheart.mp3',
	'html/music/shark.mp3',
}