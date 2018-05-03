#splits up the 500x500 .asc heightmap into an 80x80 file, to work with my level format

import os

curDir = os.path.dirname(os.path.abspath(__file__))

heightmap = []
heightmapfile = open(os.path.join(curDir, "heightmap.txt"), "w+")

with open(os.path.join(curDir, "sm9637_dsm_2m.asc"),'r') as f:
    for line in f:
        for word in line.split():
           heightmap.append(word)
		   
for y in range(0,80,1):
	for x in range(0,80,1):
		heightmapfile.write(heightmap[int(x * (500 / 80)) + int(y * (500 / 80)) * 500])
		if(x < 79):
			heightmapfile.write(" ")
	heightmapfile.write("\n")