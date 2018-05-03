using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    CharacterController m_controller;
    public float moveSpeed, lookSpeed;

	// Use this for initialization
	void Start () {
        m_controller = GetComponent<CharacterController>();
	}

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxis("Vertical") * moveSpeed;
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * lookSpeed, 0.0f));
        Vector3 moveDirection = transform.forward * speed;

        m_controller.Move(moveDirection * Time.deltaTime);
        m_controller.Move(new Vector3(0.0f, -10.0f) * Time.deltaTime);
    }
}
