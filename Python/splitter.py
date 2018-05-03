#used to split up the images to run the training on the model

from PIL import Image
import os

def crop(Path, input, height, width, folder):
	k = 1
	im = Image.open(input)
	imgwidth, imgheight = im.size
	for i in range(0,imgheight,height):
		for j in range(0,imgwidth,width):
			print ("{} {}".format(i, j))
			box = (j, i, j+width, i+height)
			a = im.crop(box)
			a.save(os.path.join(Path,"%s" % folder,"IMG-%s.jpg" % k))
			k +=1
			
curDir = os.path.dirname(os.path.abspath(__file__))

# the image to split up, in the same directory as this script
curImage = "imageToSplit.png"

# the folder to save the images to
saveFolder = "folderName"

crop(curDir, os.path.join(curDir, curImage), 32, 32, saveFolder)
input('image successfully split, press ENTER to exit')