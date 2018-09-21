using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BeginningController : MonoBehaviour {

    [SerializeField]
    private TextMeshPro text1;
    [SerializeField]
    private TextMeshPro text2;
    [SerializeField]
    private TextMeshPro text3;
    [SerializeField]
    private TextMeshPro text4;

    private string text1String = "You are a worker at an electronics manufacturing factory.";
    private string text2String = "Your day begins in your dorm, owned by the factory.";
    private string text3String = "You get ready to leave your dorm and go to work at the factory.";
    private string text4String = "Press the grab button to begin.";

    private bool allowContinue = false;
    private bool buttonPressed = false;
   
    // Use this for initialization
    void Start()
    {
        text1.SetText(text1String);
        text2.SetText(text2String);
        text3.SetText(text3String);
        text4.SetText(text4String);

        StartCoroutine(FadeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(allowContinue)
        {
            if (!buttonPressed && (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)))
            {
                buttonPressed = true;
                SceneManager.LoadScene(1);
            }
        }
    }

    IEnumerator FadeRoutine()
    {
        StartCoroutine(FadeInText(text1, 2.0f));
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(FadeInText(text2, 2.0f));
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(FadeInText(text3, 2.0f));
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(FadeInText(text4, 2.0f));
        allowContinue = true;

    }

    IEnumerator FadeInText(TextMeshPro textMesh, float time)
    {
        Debug.Log("Fading in: " + textMesh);
        float targetAlpha = 1.0f;
        float currentAlpha = 0.0f;

        float elapsedTime = 0.0f;

        while (elapsedTime <= time)
        {
            Color newColor = new Color(155, 23, 255, Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / time));
            textMesh.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
