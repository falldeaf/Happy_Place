led("purple")
write1(" Assorted Poems\n ")
write2("Press left and right to pick new poems and up and down to scroll through them.")
const increment = 111

poem_list = []
poem_list[0] = "'Habituation' --Hala Alyan\n\n today a woman tucked me between her legs like an egg // of course I think egg // she tells me each chakra is blocked // it feels prickly, she says of the hand between // my hipbones // you can pray now, // she says of the hand // on my forehead // what I don’t ask is // when will this heart boat itself across the ocean // when will this heat break // I want a winter twice as long as summer and I applaud the flock // of geese pulling the night sky like a white thread // ask me about habituation // and I’ll show you Paris in July // how the days noosed me like a turtleneck // each dawn a misfiring of cortisol // listen / I threw a silk dress over the balcony // onto a street in Montmartre // isn’t that another way of saying I need this, too? // please don’t misunderstand me // my husband sings and I fall to my knees // I should know better at this point // than to believe my own body // but hasn’t the story already changed because I told it // don’t I circle my life like a vulture for sound bites // the hot black of a movie theater // panic-bent over the sink // the water glass in four pieces // the fist I recognized in the dream // what would you tell her, the woman asks // about my own shivering body on that bed // I’d say you wanted enlightenment // did you think you’d find it at the bodega // next to the sunflowers // I’d say pay attention // I’d say wipe your face // get some rest // you’re going to need it // I’d say you said you were ready // so show me"
poem_list[1] = "'Untitled' --James Baldwin Lord\n\n when you send the rain think about it, please, a little? Do not get carried away by the sound of falling water, the marvelous light on the falling water. I am beneath that water. It falls with great force and the light Blinds me to the light."
poem_list[2] = "'Oh Wonder' --Traci Brimhall\n\n It's the garden spider who eats her mistakes at the end of day so she can billow in the lung of night, dangling from an insecure branch or caught on the coral spur of a dove's foot, and sleep, her spinnerets trailing radials like ungathered hair. It's a million-pound cumulus. It's the troposphere, holding it, miraculous. It's a mammatus rolling her weight through dusk waiting to unhook and shake free the hail. Sometimes it's so ordinary it escapes your notice— pothos reaching for windows, ease of an avocado slipping its skin. A porcelain boy with lampblack eyes told me most mammals have the same average number of heartbeats in a lifetime. It is the mouse engine that hums too hot to last. It is the blue whale's slow electricity—six pumps per minute is the way to live centuries. I think it's also the hummingbird I saw in a video, lifted off a cement floor by firefighters and fed sugar water until she was again a tempest. It wasn't when my mother lay on the garage floor and my brother lifted her while I tried to shout louder than her sobs. But it was her heart, a washable ink. It was her dark's genius, how it moaned slow enough to outlive her. It is the orca who pushes her dead calf a thousand miles before she drops it or it falls apart. And it is also when she plays with her pod the day after. It is the night my son tugs at his pajama collar and cries: The sad is so big I can't get it all out, and I behold him, astonished, his sadness as clean and abundant as spring. His thunder-heart, a marvel I refuse to invade with empathy. And outside, clouds groan like gods, a garden spider consumes her home. It's knowing she can weave it tomorrow between citrus leaves and earth. It's her chamberless heart cleaving the length of her body. It is lifting my son into my lap to witness the birth of his grieving."

poem_index = 0
page_index = 0

function action(a) {
	switch(a) {
		case "left_click":
		page_index = 0
			poem_index--
			if(poem_index<0) poem_index = poem_list.length-1
			updatescreens()
			break
		case "right_click":
			page_index = 0
			poem_index++;
			if(poem_index>poem_list.length-1) poem_index = 0
			updatescreens()
			break
		case "top_click":
			if(page_index>0) {
				page_index -= 2
				updatescreens()
			}
	break
		case "bottom_click":
		if( (page_index*increment)+(increment*2) <= poem_list[poem_index].length) {
				page_index += 2
				updatescreens()
	}
			break
	}
}

function updatescreens() {
	write1("<mspace=32>" + poem_list[poem_index].substr(page_index*increment, increment) + "</mspace>")
	write2("<mspace=32>" + poem_list[poem_index].substr(page_index*increment+increment, increment) + "</mspace>")
}