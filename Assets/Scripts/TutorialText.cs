using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// give this to a TextMeshPro object to make it stick to another gameobject
// and rotate to face the player

public class TutorialText : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject anchor;

	// Use this for initialization
	void Start () {
        //
        //Renderer renderer = anchor.GetComponentInChildren<Renderer>();
        //RectTransform rectTransform = GetComponentInChildren<RectTransform>();
        //TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        //textMeshPro.fontSize = 8f;
        //textMeshPro.autoSizeTextContainer = true;

        //rectTransform.localScale = new Vector3(-1f, 1f, 1f);

        //Vector3 anchorOffset = new Vector3(0f, renderer.bounds.extents.y + rectTransform.rect.height / 2, 0f);
        //Debug.Log("I tried to put it units up: " + renderer.bounds.extents.y);
        // if the text is a child then you dont need to specify first position
        //gameObject.transform.localPosition = anchor.transform.position + anchorOffset; //anchor.transform.position + anchorOffset;


    }

    // Update is called once per frame
    void Update () {
		
	}

    private void FixedUpdate()
    {
        gameObject.transform.LookAt(player.transform);
    }
}
