setup("weather")
led("purple")
write1("I'm the Weather controller!")
write2("Time to get stormy!")
log("I'm the weather program!")

weather_preset(5)

function action(a) {
	log(a)
}