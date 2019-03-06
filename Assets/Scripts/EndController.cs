using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndController : MonoBehaviour {
    List<Transform> texts = new List<Transform>();

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

    private string goodPhonesString = "You put a screw into {0} phones.";
    private string goodPhoneString = "You put a screw into {0} phone.";
    private string badPhonesString = "You missed {0}.";

    private string quotaString = "";
    private string badPerformanceString =
        "You failed to meet quota. \nFor your poor performance, your manager denies you overtime and reduces your pay.";
    private string adequatePerformanceString = 
        "Your performance is deemed acceptable, and you escape punishment.";

    private string paymentString = "You perform that task for $2.91 AUD per hour, for a 10 hour shift. You are paid the equivalent of $31.97 AUD. \n\nYou performed the task for 10 minutes; every minute you spent is equal to over 1 hour of real time.";
    private string endString = "Press any key to end the simulation.";

    private bool allowEnd = false;
    // Use this for initialization
    void Start () {
        texts.Add(goodPhonesText.transform);
        texts.Add(badPhonesText.transform);
        texts.Add(performanceText.transform);
        texts.Add(paymentText.transform);
        texts.Add(endText.transform);

        if (Stats.Goodphones == 1)
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
        

        StartCoroutine(FadeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (allowEnd)
        {
            if (Input.anyKeyDown)
            {
                Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd82WRkfuTBm_jpC-FwyTqljQEthD_TNo9Wm6v6WHH8jIrvsg/viewform?usp=sf_link");
                Application.Quit();

            }
        }
        foreach (Transform text in texts)
        {
            // set it to be in front of the ovrcamera
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
