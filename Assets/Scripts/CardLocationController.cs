using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLocationController : MonoBehaviour {
    [SerializeField]
    private GameObject camera;
	
	// Update is called once per frame
	void Update () {
        Vector3 yOffset = new Vector3(-0.2f, 0.0f, -0.1f);
        
        transform.position = camera.transform.position + yOffset;
	}
}
