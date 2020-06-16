setup("oggplayer")
led("purple")
write1("I'm the ogg player!")
write2("Time to listen to music!")

playlist_index = 0
picksong(playlist_index, 0)
//playlist = getPlaylist()

function action(a) {
	log(a)
	switch(a) {
		case "left_click":
			log(playlist[playlist_index].songname)
			playlist_index--;
			if(playlist_index<0) playlist_index = playlist.length-1
			picksong(playlist_index, 0)
			break;
		case "right_click":
			log(playlist[playlist_index].songname)
			playlist_index++;
			if(playlist_index>playlist.length-1) playlist_index = 0
			picksong(playlist_index, 0)
			break;
		case "top_click":
			play(playlist_index)
			break;
		case "bottom_click":
			stop()
			break;
	}
}

function currenttime(t) {
	picksong(i, t)
}

function picksong(i, p) {
	write1("Title: " + playlist[i].songname + "\n" +
			"Artist: " + playlist[i].artist + "\n" +
			"Album: " + playlist[i].album + "\n\n"
		)
}