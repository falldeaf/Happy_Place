setup("weather")
led("blue")

weather_busy = false
weather_index = 0
write1(weather_list[weather_index])
write2("Weather Controls")

function action(a) {
	log(a)
	switch(a) {
		case "left_click":
			weather_index--
			if(weather_index<0) weather_index = weather_list.length-1
			write1(weather_list[weather_index])
			break
		case "right_click":
			weather_index++;
			if(weather_index>weather_list.length-1) weather_index = 0
			write1(weather_list[weather_index])
			break
		case "bottom_click":
			if(!weather_busy) {
				led("red")
				weather_busy = true;
				wait(62.0, "weatherdone");
				switchweather(weather_index)
			}
			break
	}
}

function weatherdone() {
	log("is this getting called?")
	led("blue")
	weather_busy = false;
}