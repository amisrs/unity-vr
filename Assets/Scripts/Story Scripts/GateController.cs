using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class GateController : MonoBehaviour {
    
    [SerializeField]
    private GameObject pivot;
    [SerializeField]
    private GameObject flap;
    [SerializeField]
    private GameObject tutorialText;
    [SerializeField]
    private GameObject tapText;
    [SerializeField]
    private Vector3 plateOffset;
    [SerializeField]
    private AudioSource beep;

    [SerializeField]
    private TextMeshProUGUI instruction;
    [SerializeField]
    private TextMeshProUGUI instructionVR;

    

    private string instruction1 = "Tap on to the gates";
    private string instruction2 = "Enter the factory";
   

    private Quaternion targetRotation;
    private bool isGateOpen = false;


	// Use this for initialization
	void Start () {
        //StartCoroutine(OpenGate());
        //tapText.SetActive(true);
        if(XRSettings.enabled)
        {
            instructionVR.SetText(instruction1);
        } else
        {
            instruction.SetText(instruction1);
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void openGate()
    {
        if(!isGateOpen)
        {
            if(XRSettings.enabled)
            {
                instructionVR.SetText(instruction2);
            } else
            {
                instruction.SetText(instruction2);
            }          
            tutorialText.SetActive(false);
            StartCoroutine(OpenGate());
            isGateOpen = true;
            tapText.transform.position = gameObject.transform.position + plateOffset;

            tapText.SetActive(true);
            beep.Play();
            
        }
    }

    IEnumerator OpenGate()
    {
        float anglePerFrame = 1.0f;
        float totalAngle = 0.0f;

        while (totalAngle < 90.0f)
        {
            anglePerFrame = Mathf.LerpAngle(anglePerFrame, 90.0f, 0.0015f);
            flap.transform.RotateAround(pivot.transform.position, pivot.transform.up, anglePerFrame);
            totalAngle += anglePerFrame;
            yield return new WaitForEndOfFrame();
        }
    }
}
