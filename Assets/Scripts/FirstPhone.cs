using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPhone : MonoBehaviour
{

    [SerializeField]
    FactoryScript factoryScript;
    [SerializeField]
    GameObject phoneLocation;

    private ScrewSlotController screwSlot;
    private bool isStarted;

    // Use this for initialization
    void Start()
    {
        isStarted = false;
        screwSlot = GetComponentInChildren<ScrewSlotController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScrewPlaced()
    {
        factoryScript.FirstScrewPlaced();
    }

    public void ScrewTurned()
    {
        factoryScript.ScrewTurned();
        factoryScript.screwedPhone();
    }

    public void ReturnHome()
    {
        if (!isStarted)
        {
            StartCoroutine(MoveToPosition(gameObject, 1.0f));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isStarted && collision.gameObject.layer == 9 && screwSlot.isScrewScrewed()) // conveyorlayer
        {
            factoryScript.countPracticePhone();
            isStarted = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14) // floor layer
        {
            StartCoroutine(MoveToPosition(gameObject, 1.0f));
        }
    }


    IEnumerator MoveToPosition(GameObject phone, float time)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            phone.transform.position = Vector3.Lerp(phone.transform.position, phoneLocation.transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

}
