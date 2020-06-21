score = 0
player = [1,1]
enemy1 = [1,1]
enemy2 = [1,1]
playing = true

grid_blank = [
				[ "╔", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "╗" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "╚", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "╝" ]
]
grid_visual = [
				[ "╔", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "╗" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "║", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "║" ],
				[ "╚", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "═", "╝" ]
]

function blank() {
	for(i=0; i<grid_blank.length; i++) {
		for(j=0; j<grid_blank[0].length; j++) {
			grid_visual[i][j] = grid_blank[i][j]
		}
	}
}

function draw() {
	blank()
	grid_visual[player[0]][player[1]] = "☻"
	grid_visual[enemy1[0]][enemy1[1]] = "☺"
	grid_visual[enemy2[0]][enemy2[1]] = "☺"

	str = ""
	for(i=0; i<grid_visual.length; i++) {
		for(j=0; j<grid_visual[0].length; j++) {
			str += grid_visual[i][j]
		}
		str += "\n"
	}
	write1("<mspace=44>" + str + "</mspace>")
}

function restart() {
	score = 0
	player[0] = getRandomInt(1,6)
	player[1] = getRandomInt(1,15)
	enemy1[0] = getRandomInt(1,6)
	enemy1[1] = getRandomInt(1,15)
	enemy2[0] = getRandomInt(1,6)
	enemy2[1] = getRandomInt(1,15)
	draw()
	write2("They're after you! Click up, down, left, or right to run!!!")
}

function getRandomInt(min, max) {
	min = Math.ceil(min);
	max = Math.floor(max);
	return Math.floor(Math.random() * (max - min)) + min; //The maximum is exclusive and the minimum is inclusive
}

function moveenemies() {
	if(player[0]<enemy1[0]) {
		if(!cc([enemy1[0]-1, enemy1[1]], enemy2)) enemy1[0]--
	} else if(player[0]>enemy1[0]) {
		if(!cc([enemy1[0]+1, enemy1[1]], enemy2)) enemy1[0]++
	} else {
		if(player[1]<enemy1[1]) {
			if(!cc([enemy1[0], enemy1[1]-1], enemy2)) enemy1[1]--
		}
		if(player[1]>enemy1[1]) {
			if(!cc([enemy1[0], enemy1[1]+1], enemy2)) enemy1[1]++
		}
	}

	if(player[1]<enemy2[1]) {
		if(!cc([enemy2[0], enemy2[1]-1], enemy1)) enemy2[1]--
	} else if(player[1]>enemy2[1]) {
		if(!cc([enemy2[1], enemy2[1]+1], enemy1)) enemy2[1]++
	} else {
		if(player[0]<enemy2[0]) {
			if(!cc([enemy2[0]-1, enemy2[1]], enemy1)) enemy2[0]--
		}
		if(player[0]>enemy2[0]) {
			if(!cc([enemy2[0]+1, enemy2[1]], enemy1)) enemy2[0]++
		}
	}
	write2("Moves: " + score)
}

function cc(me, you) {
	return (me[0] == you[0] && me[1] == you[1])
} 

function action(a) {
	if(!playing) {
		playing = true
		restart()
	}

	switch(a) {
		case "top_click":
			if(player[0]>1) player[0]--
			break
		case "bottom_click":
			if(player[0]<5) player[0]++
			break
		case "left_click":
			if(player[1]>1) player[1]--
			break
		case "right_click":
			if(player[1]<14) player[1]++
			break
	}

	moveenemies()
	draw()
	if(cc(player, enemy1) || cc(player, enemy2)) {
		lose() 
	} else {
		score++
	}
}

function lose() {
	playing = false
	write2("You've been captured! " + score + " \n\n Click to try again.")
}

restart()