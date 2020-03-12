using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableZoneController : MonoBehaviour {

    [SerializeField]
    private GameObject screwdriverLocation;

    private bool movementStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Screwdriver") && !movementStarted)
        {
            MouseGrabbable mouseGrabbable = other.gameObject.GetComponent<MouseGrabbable>();
            if (mouseGrabbable.m_grabbedBy == null)
            {
                StartCoroutine(MoveToPosition(other.gameObject, 1.0f));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Screwdriver") && !movementStarted)
        {
            MouseGrabbable mouseGrabbable = other.gameObject.GetComponent<MouseGrabbable>();
            if (mouseGrabbable.m_grabbedBy == null)
            {
                StartCoroutine(MoveToPosition(other.gameObject, 1.0f));
            }
        }

    }



    IEnumerator MoveToPosition(GameObject screwdriver, float time)
    {
        float elapsedTime = 0.0f;
        movementStarted = true;

        while (elapsedTime < time)
        {
            screwdriver.transform.position = Vector3.Lerp(screwdriver.transform.position, screwdriverLocation.transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        movementStarted = false;

    }

}
