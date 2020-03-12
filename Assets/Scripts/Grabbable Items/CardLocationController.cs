using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CardLocationController : MonoBehaviour {
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private Vector3 yOffsetVR = new Vector3(-0.59f, 0.0f, -0.62f);

    [SerializeField]
    private Vector3 yOffset = new Vector3(0.17f, -0.32f, -0.08f);

    // Update is called once per frame
    void Update () {
        if(XRSettings.enabled)
        {
            transform.position = camera.transform.position + yOffsetVR;
        } else
        {
            transform.position = camera.transform.position + yOffset;

        }

    }
}
