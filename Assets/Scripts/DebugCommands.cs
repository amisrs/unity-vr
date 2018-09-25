using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DebugCommands : MonoBehaviour {

    [SerializeField]
    private GameObject vrPlayer;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject teleporter;
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject card;

    [SerializeField]
    private OVRGrabbable[] oVRGrabbables;
    [SerializeField]
    private MouseGrabbable[] mouseGrabbables;

	// Use this for initialization
	void Start () {

        Cursor.lockState = CursorLockMode.Locked;
        if(XRSettings.enabled && canvas.activeInHierarchy) {
            canvas.SetActive(false);
        }

        if(XRSettings.enabled) // it usually starts with vr enabled
        {
            player.SetActive(false);
            ToggleMouseComponents(false);
            ToggleOVRComponents(true);
        } else
        {
            vrPlayer.SetActive(false);
            ToggleMouseComponents(true);
            ToggleOVRComponents(false);
        }
        
        oVRGrabbables = FindObjectsOfType<OVRGrabbable>();
        mouseGrabbables = FindObjectsOfType<MouseGrabbable>();
        SetVREnabled(XRSettings.enabled);

    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.F1))
        {
            SetVREnabled(!XRSettings.enabled);
        }
	}

    void SetVREnabled(bool enabled)
    {
        XRSettings.enabled = enabled;
        Debug.Log("XRSettings: " + XRSettings.enabled);

        if (!XRSettings.enabled && teleporter.activeInHierarchy)
        {
            teleporter.SetActive(false);
        }

        if (XRSettings.enabled && !teleporter.activeInHierarchy)
        {
            teleporter.SetActive(true);
        }

        if(XRSettings.enabled && canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
        }
        if(!XRSettings.enabled && !canvas.activeInHierarchy)
        {
            canvas.SetActive(true);
        }

        if(!XRSettings.enabled)
        {
            Debug.Log("Deactivating VR");
            ToggleOVRComponents(false);
            ToggleMouseComponents(true);
            vrPlayer.SetActive(false);
            player.SetActive(true);
        }
        else
        {
            Debug.Log("Activating VR");
            ToggleMouseComponents(false);
            ToggleOVRComponents(true);
            player.SetActive(false);
            vrPlayer.SetActive(true);

        }
    }

    void ToggleOVRComponents(bool state)
    {
        foreach (OVRGrabbable grabbable in oVRGrabbables)
        {
            Debug.Log("Setting " + grabbable.name + " : " + state);
            grabbable.enabled = state;
        }
    }

    void ToggleMouseComponents(bool state)
    {
        foreach (MouseGrabbable grabbable in mouseGrabbables)
        {
            grabbable.enabled = state;
        }
    }
}
