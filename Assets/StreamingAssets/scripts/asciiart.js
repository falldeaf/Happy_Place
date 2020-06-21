led("purple")
write1("")
write2("Welcome to the ascii Art gallery! \\(^O^)/\n\nClick left or right to view ASCII Art.")

art_index = 0
artlist = [
"\
 ____ ___   jgs\n\
 )  =\\  =\\\n\
/    =\\  =\\\n\
\\      `-._`-._\n\
 )__(`\\____)___)",

"   _.........._\n\
  | |mga     | | \n\
  | |        | | \n\
  | |________| | \n\
  |   ______   | \n\
  |  |    | |  | \n\
  |__|____|_|__| ",

"      ____ \n\
    ,'   Y`. \n\
   /        \\ \n\
   \\ ()  () / \n\
    `. /\\ ,' \n\
8====| '' |====8\n\
     `LLLU'",

"      ,~~. \n\
     (  9 )-_,\n\
(\\___ )=='-'\n\
 \\ .   ) )\n\
  \\ `-' /\n\
   `~j-'   hjw\n\
     '=:",

"  A_A\n\
 (-.-)\n\
  |-|\n\
 /   \\\n\
|     | \n\
|  || |  /\\__\n\
 \\_||_/_/"
]

function action(a) {
	log(a)
	switch(a) {
		case "left_click":
			art_index--;
			if(art_index<0) art_index = artlist.length-1
			write1(artlist[art_index])
			break
		case "right_click":
			art_index++;
			if(art_index>artlist.length-1) art_index = 0
			write1(artlist[art_index])
			break
	}
}