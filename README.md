## Satellite Levels

### About
Turns satellite images into level files, parsed in Unity. Uses the TensorFlow library in Python for image recognition.
Created during my third year of university for an Advanced Technologies module.

### Instructions
The project as-is includes a few example levels that have already been parsed from images. These can be read with the Unity project.
To create more, modify the 'classify_image' script and change the 'curImage' and 'saveFolder' variables.
Running the script will then scan the given image if it can find it, and save the resulting file.

This process can take a fairly long time to complete, often up to an hour if not longer.
Heightmaps were hard to obtain, and only one example is supplied in 'map1'.

### Examples

![](http://www.synert.co.uk/images/code/satellite3.png)

[Comparison video 1](https://www.youtube.com/watch?v=HS3qJjsYlig)

[Comparison video 2](https://www.youtube.com/watch?v=YvwTiGNEtgg)