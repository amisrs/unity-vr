using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private string goodPhonesString = "You put a screw into {0} phones.";
    private string goodPhoneString = "You put a screw into {0} phone.";
    private string badPhonesString = "You missed {0}.";

    private string quotaString = "";
    private string badPerformanceString =
        "You failed to meet quota. \nFor your poor performance, your manager denies you overtime and reduces your pay.";
    private string adequatePerformanceString = 
        "Your performance is deemed acceptable, and you escape punishment.";

    private string paymentString = "Performing that task for an 11 hour shift, you are paid the equivalent of $31.97 AUD.";
    private string endString = "End of simulation.";
    // Use this for initialization
    void Start () {
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

        StartCoroutine(FadeRoutine());
    }
	
	// Update is called once per frame
	void Update () {
		
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
