using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

// give this to a TextMeshPro object to make it stick to another gameobject
// and rotate to face the player

public class TutorialText : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerVR;

    private GameObject realPlayer;


    // Use this for initialization
    void Start()
    {
        if (XRSettings.enabled)
        {
            realPlayer = playerVR;
        }
        else
        {
            realPlayer = player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (XRSettings.enabled)
        {
            realPlayer = playerVR;
        }
        else
        {
            realPlayer = player;
        }
    }
    public void SetText(string text)
    {
        transform.Find("Content").GetComponentInChildren<TextMeshPro>().SetText(text);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Looking at object: " + realPlayer.name);
        gameObject.transform.LookAt(realPlayer.transform);
    }
}
