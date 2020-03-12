using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkAreaController : MonoBehaviour {

    [SerializeField]
    private GameObject screwdriverLocation;


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Screwdriver"))
        {
            StartCoroutine(MoveToPosition(other.gameObject, 1.0f));
        }
    }

    IEnumerator MoveToPosition(GameObject screwdriver, float time)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            screwdriver.transform.position = Vector3.Lerp(screwdriver.transform.position, screwdriverLocation.transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

}
