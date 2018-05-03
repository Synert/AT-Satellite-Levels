using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile : MonoBehaviour {

    public Texture2D dirt, grass, road, water;
    public Transform[] tree, building;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(LoadLevel.Tile data, bool corner)
    {
        if (data.height < -2) data.height = 0;
        transform.position += new Vector3(0, data.height, 0);
        if (data.height > 0)
        {
            transform.localScale += new Vector3(0, data.height / 2, 0);
        }

        data.tex1 = data.tex1.Trim();
        data.tex2 = data.tex2.Trim();
        data.obj = data.obj.Trim();
        AddBlend("_Blend", 1.0f - (data.blend / 100.0f));
        if (data.tex1 == "dirt") AddTexture("_MainTex", dirt);
        if (data.tex1 == "grass") AddTexture("_MainTex", grass);
        if (data.tex1 == "roads") AddTexture("_MainTex", road);
        if (data.tex1 == "water")
        {
            Destroy(gameObject);
            return;
        }

        if (data.tex2 == "dirt") AddTexture("_Texture2", dirt);
        if (data.tex2 == "grass") AddTexture("_Texture2", grass);
        if (data.tex2 == "roads") AddTexture("_Texture2", road);
        if (data.tex2 == "water") AddTexture("_Texture2", water);

        if (corner) return;

        if(data.obj == "buildings")
        {
            Transform newBuilding = Instantiate(building[Random.Range(0, building.Length)]);
            newBuilding.position = transform.position;
            //newBuilding.position += new Vector3(Random.Range(-0.15f, 0.15f), 0, Random.Range(-0.15f, 0.15f));
            newBuilding.localScale = new Vector3((data.objSize + 100.0f) / 200.0f, (data.objSize + 100.0f) / 200.0f * Random.Range(1.0f, 3.0f), (data.objSize + 100.0f) / 200.0f);
        }
        if(data.obj == "trees")
        {
            for (int manyTrees = 0; manyTrees < Random.Range(1, 2); manyTrees++)
            {
                Transform newTree = Instantiate(tree[Random.Range(0, tree.Length)]);
                newTree.position = transform.position;
                newTree.position += new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f));
                Vector3 newRot = newTree.rotation.eulerAngles;

                newRot.y = Random.Range(0.0f, 360.0f);
                newTree.rotation = Quaternion.Euler(newRot);

                float scalar = 1.25f / (float)(manyTrees + 1);
                scalar *= (data.objSize / 100.0f);

                newTree.localScale = new Vector3(scalar, scalar, scalar);
            }
        }
    }

    void AddTexture(string set, Texture2D tex)
    {
        foreach(Renderer mat in GetComponentsInChildren<Renderer>())
        {
            mat.material.SetTexture(set, tex);
        }
    }

    void AddBlend(string set, float blend)
    {
        foreach (Renderer mat in GetComponentsInChildren<Renderer>())
        {
            mat.material.SetFloat(set, blend);
        }
    }
}
