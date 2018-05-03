using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBob : MonoBehaviour {

    public float timeOffset = 0f;
    public float bobDistance = 0.25f;
    public float bobSpeed = 1f;

    private float orig_y;

	// Use this for initialization
	void Start () {
        orig_y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0.0f, orig_y + Mathf.Sin(Time.fixedTime * bobSpeed + timeOffset) * bobDistance, 0.0f);
	}
}
