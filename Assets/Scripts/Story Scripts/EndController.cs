using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using TMPro;

public class EndController : MonoBehaviour {
    [SerializeField]
    private TextMeshPro goodPhonesText;
    [SerializeField]
    private TextMeshPro badPhonesText;
    [SerializeField]
    private TextMeshPro performanceText;
    [SerializeField]
    private TextMeshPro paymentText;
    [SerializeField]
    private TextMeshPro endText;
    [SerializeField]
    private Transform textWall;
    [SerializeField]
    public OVRCameraRig vrCameraRig;
    public float textDistance = 100f;

    private string formURL = "";

    private string goodPhonesString = "You put a screw into {0} phones.";
    private string goodPhoneString = "You put a screw into {0} phone.";
    private string badPhonesString = "You missed {0}.";

    private string quotaString = "";
    private string badPerformanceString =
        "You failed to meet quota. \nFor your poor performance, your manager denies you overtime and reduces your pay.";
    private string adequatePerformanceString = 
        "Your performance is deemed acceptable, and you escape punishment.";

    private string paymentString = "You perform that task for $2.91 AUD per hour, for an 11 hour shift. You are paid the equivalent of $31.97 AUD. \n\nYou performed the task for 10 minutes; every minute you spent is equal to over 1 hour of real time.";
    private string endString = "Press any key to end the simulation.";

    private bool allowEnd = false;

    private Vector3 textWallOffset;
    // Use this for initialization
    void Start () {
        vrCameraRig = FindObjectOfType<OVRCameraRig>();

        if(XRSettings.enabled)
        {
            textDistance = 100f;
        } else
        {
            textDistance = 60f;
        }

        textWall = goodPhonesText.transform.parent;
        textWallOffset = textWall.position - vrCameraRig.transform.position;
        
        //textWall.transform.parent = vrCameraRig.transform;
        textWall.position = vrCameraRig.transform.position;
        if(Stats.Goodphones == 1)
        {
            goodPhonesText.SetText(goodPhoneString, Stats.Goodphones);
        } else
        {
            goodPhonesText.SetText(goodPhonesString, Stats.Goodphones);
        }
        
        badPhonesText.SetText(badPhonesString, Stats.Badphones);

        if(Stats.Victory)
        {
            performanceText.SetText(adequatePerformanceString);
        } else
        {
            performanceText.SetText(badPerformanceString);
        }

        paymentText.SetText(paymentString);
        endText.SetText(endString);

        string formURLFile = Path.GetDirectoryName(Application.dataPath) + "/form.txt";
        string outputFile = Path.GetDirectoryName(Application.dataPath) + "/output.txt";
        try
        {
            StreamWriter writer = new StreamWriter(outputFile, true, System.Text.Encoding.UTF8);
            writer.Write("[" + System.DateTime.Now + "]" + Stats.Goodphones.ToString() + "\n");
            writer.Close();
        } catch (IOException e)
        {
            Debug.Log(e);
        }

        Debug.Log(formURLFile);
        try
        {
            StreamReader reader = new StreamReader(formURLFile);
            formURL = reader.ReadToEnd();
            reader.Close();
            Debug.Log(formURL);
        } catch (FileNotFoundException e)
        {
            Debug.Log("No form text file.");
        }
        

        StartCoroutine(FadeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(vrCameraRig.transform.position, vrCameraRig.centerEyeAnchor.transform.forward * 10, Color.green);
        textWall.position =  (vrCameraRig.transform.position - vrCameraRig.centerEyeAnchor.transform.forward * -textDistance);
        textWall.LookAt(vrCameraRig.transform);
        Debug.DrawLine(vrCameraRig.transform.position, vrCameraRig.transform.position- vrCameraRig.centerEyeAnchor.transform.forward * 10, Color.red);

        if (allowEnd)
        {
            if (Input.anyKeyDown)
            {
                if(formURL.EndsWith("=") && formURL.Contains("goo"))
                {
                    Application.OpenURL(formURL + Stats.Goodphones);
                } else
                {
                    Application.OpenURL(formURL);
                }
                
                Application.Quit();

            }
        }
    }

    IEnumerator FadeRoutine()
    {
        StartCoroutine(FadeInText(goodPhonesText, 2.0f));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeInText(badPhonesText, 2.0f));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeInText(performanceText, 2.0f));
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(FadeInText(paymentText, 2.0f));
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(FadeInText(endText, 2.0f));
        allowEnd = true;
    }

    IEnumerator FadeInText(TextMeshPro textMesh, float time)
    {
        Debug.Log("Fading in: " + textMesh);
        float targetAlpha = 1.0f;
        float currentAlpha = 0.0f;

        float elapsedTime = 0.0f;

        while(elapsedTime <= time)
        {
            Color newColor = new Color(155, 23, 255, Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / time));
            textMesh.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


}
