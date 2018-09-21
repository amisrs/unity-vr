using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   

    private Quaternion targetRotation;
    private bool isGateOpen = false;


	// Use this for initialization
	void Start () {
        //StartCoroutine(OpenGate());
        //tapText.SetActive(true);

    }

    // Update is called once per frame
    void Update () {

    }

    public void openGate()
    {
        if(!isGateOpen)
        {
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
