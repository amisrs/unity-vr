using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableHighlighter : MonoBehaviour {

    // put this on grabvolumebig


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<OVRGrabbable>() != null)
        {
            Renderer renderer = other.gameObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                Material[] materials = renderer.materials;
                foreach (Material mat in materials)
                {
                    mat.shader = Shader.Find("Outlined/UltimateOutline");

                    if (gameObject.layer == 11) //hell yeah hardcode screweditem layer
                    {
                        mat.SetFloat("_FirstOutlineWidth", 2.0f);
                    }

                }
            }                
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<OVRGrabbable>() != null)
        {
            Renderer renderer = other.gameObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                Material[] materials = other.gameObject.GetComponent<Renderer>().materials;
                foreach (Material mat in materials)
                {
                    mat.shader = Shader.Find("Standard");
                }
            }

        }
    }
}
