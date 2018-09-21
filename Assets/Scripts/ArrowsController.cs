using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just make it spin

public class ArrowsController : MonoBehaviour {
    [SerializeField]
    private float zRotation = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(new Vector3(0f, 0f, -zRotation));		
	}
}
