using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public struct Tile
    {
        public string tex1, tex2, obj;
        public float blend, objSize, height;
    }

    Tile[] level;
    int width, height;

    public string levelFile, heightmapFile;
    public Transform levelTile;
    public Transform bottomLeft, bottomRight, topLeft, topRight;
    public Transform bigBottomLeft, bigBottomRight, bigTopLeft, bigTopRight;
    public float maxHeight = 10.0f;

	// Use this for initialization
	void Start () {
        Load();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Load()
    {
        if (levelFile != "")
        {
            StreamReader file = new StreamReader(levelFile);
            string[] words = file.ReadToEnd().Split(new char[] {' ', '\n' });
            StreamReader hmap;
            string[] moreWords = null;
            if (heightmapFile != "")
            {
                hmap = new StreamReader(heightmapFile);
                moreWords = hmap.ReadToEnd().Split(new char[] { ' ', '\n' });
            }
            width = (int)float.Parse(words[0]);
            height = (int)float.Parse(words[1]);
            level = new Tile[width * height];

            Debug.Log("Starting level load");
            int toAdd = 2;
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    level[y * width + x].tex1 = words[y * width + x + toAdd].Trim();
                    level[y * width + x].tex2 = words[y * width + x + toAdd + 1].Trim();
                    level[y * width + x].blend = float.Parse(words[y * width + x + toAdd + 2]);
                    level[y * width + x].obj = words[y * width + x + toAdd + 3].Trim();
                    level[y * width + x].objSize = float.Parse(words[y * width + x + toAdd + 4]);
                    if (heightmapFile != "")
                    {
                        Debug.Log(moreWords[y * width + x]);
                        level[y * width + x].height = float.Parse(moreWords[y * width + x]) * (maxHeight / 100.0f);
                    }
                    else
                    {
                        level[y * width + x].height = 0;
                    }
                    toAdd += 4;
                }
            }
            Debug.Log("Level load complete, creating objects");
            Create();
        }
    }

    public void Create()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Transform newTile = Instantiate(levelTile);
                newTile.position = new Vector3(x, 0, -y);
                newTile.GetComponent<LevelTile>().Setup(level[x + y * width], false);

                if(level[x + y * width].tex1 == "water")
                {
                    //empty tile, do the thing
                    CheckNeighboursEmpty(x, y);
                }
                //else CheckNeighboursFull(x, y);

            }
        }
    }

    void CheckNeighboursEmpty(int x, int y)
    {
        bool top, left, right, bottom;
        top = left = right = bottom = false;
        int count = 0;
        //check left
        if(x > 0)
        {
            if (level[(x - 1) + y * width].tex1 != "water")
            {
                left = true;
                count++;
            }
        }
        //check right
        if (x < width - 1)
        {
            if (level[(x + 1) + y * width].tex1 != "water")
            {
                right = true;
                count++;
            }
        }
        //check down
        if (y > 0)
        {
            if (level[x + (y - 1) * width].tex1 != "water")
            {
                top = true;
                count++;
            }
        }
        //check up
        if (y < height - 1)
        {
            if (level[x + (y + 1) * width].tex1 != "water")
            {
                bottom = true;
                count++;
            }
        }

        Transform newCorner = null;
        if(left && top)
        {

            if (count > 2)
            {
                newCorner = Instantiate(topLeft);
            }
            else
            {
                newCorner = Instantiate(bigTopLeft);
            }
            newCorner.position = new Vector3(x, 0, -y);
            newCorner.GetComponent<LevelTile>().Setup(level[(x - 1) + y * width], true);
        }
        if(left && bottom)
        {
            if (count > 2)
            {
                newCorner = Instantiate(bottomLeft);
            }
            else
            {
                newCorner = Instantiate(bigBottomLeft);
            }
            newCorner.position = new Vector3(x, 0, -y);
            newCorner.GetComponent<LevelTile>().Setup(level[(x - 1) + y * width], true);
        }
        if(right && top)
        {
            if (count > 2)
            {
                newCorner = Instantiate(topRight);
            }
            else
            {
                newCorner = Instantiate(bigTopRight);
            }
            newCorner.position = new Vector3(x, 0, -y);
            newCorner.GetComponent<LevelTile>().Setup(level[(x + 1) + y * width], true);
        }
        if(right && bottom)
        {
            if (count > 2)
            {
                newCorner = Instantiate(bottomRight);
            }
            else
            {
                newCorner = Instantiate(bigBottomRight);
            }
            newCorner.position = new Vector3(x, 0, -y);
            newCorner.GetComponent<LevelTile>().Setup(level[(x + 1) + y * width], true);
        }
    }

    void CheckNeighboursFull(int x, int y)
    {
        bool top, left, right, bottom, TL, TR, BL, BR;
        top = left = right = bottom = TL = TR = BL = BR = false;
        int count = 0;
        //check left
        if (x > 0)
        {
            if (level[(x - 1) + y * width].tex1 != "water")
            {
                left = true;
                count++;
            }
        }
        //check right
        if (x < width - 1)
        {
            if (level[(x + 1) + y * width].tex1 != "water")
            {
                right = true;
                count++;
            }
        }
        //check down
        if (y > 0)
        {
            if (level[x + (y - 1) * width].tex1 != "water")
            {
                top = true;
                count++;
            }
        }
        //check up
        if (y < height - 1)
        {
            if (level[x + (y + 1) * width].tex1 != "water")
            {
                bottom = true;
                count++;
            }
        }

        //check top left/right
        if (y > 0)
        {
            //top left
            if (x > 0)
            {
                if (level[(x - 1) + (y - 1) * width].tex1 != "water")
                {
                    TL = true;
                    count++;
                }
            }
            //top right
            if (x < width - 1)
            {
                if (level[(x + 1) + (y - 1) * width].tex1 != "water")
                {
                    TR = true;
                    count++;
                }
            }
        }
        //check bottom left/right
        if (y < height - 1)
        {
            //bottom left
            if (x > 0)
            {
                if (level[(x - 1) + (y + 1) * width].tex1 != "water")
                {
                    BL = true;
                    count++;
                }
            }
            //bottom right
            if (x < width - 1)
            {
                if (level[(x + 1) + (y + 1) * width].tex1 != "water")
                {
                    BR = true;
                    count++;
                }
            }
        }

        Transform newCorner = null;
        Tile tempData = level[x + y * width];
        if (left || top || TL)
        {

            newCorner = Instantiate(topLeft);
            newCorner.position = new Vector3(x, 0, -y);
            if (left)
            {
                //tempData.tex2 = level[(x - 1) + y * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
            else
            {
                //tempData.tex2 = level[x + (y - 1) * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
        }
        if (left || bottom || BL)
        {
            newCorner = Instantiate(bottomLeft);
            newCorner.position = new Vector3(x, 0, -y);
            if (left)
            {
                //tempData.tex2 = level[(x - 1) + y * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
            else
            {
                //tempData.tex2 = level[x + (y + 1) * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
        }
        if (right || top || TR)
        {
            newCorner = Instantiate(topRight);
            newCorner.position = new Vector3(x, 0, -y);
            if (right)
            {
                //tempData.tex2 = level[(x + 1) + y * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
            else
            {
                //tempData.tex2 = level[x + (y - 1) * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
        }
        if (right || bottom || BR)
        {
            newCorner = Instantiate(bottomRight);
            newCorner.position = new Vector3(x, 0, -y);
            if (right)
            {
                //tempData.tex2 = level[(x + 1) + y * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
            else
            {
                //tempData.tex2 = level[x + (y + 1) * width].tex1;
                newCorner.GetComponent<LevelTile>().Setup(tempData, true);
            }
        }
    }
}
