using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CardMouseGrabbable : MouseGrabbable {
    [SerializeField]
    private GameObject cardLocation;

    [SerializeField]
    private DormScript dormScript;
    [SerializeField]
    private bool isAttached = false;
    private Quaternion rotation = Quaternion.Euler(80.08701f, 94.552f, 69.333f);

    // Use this for initialization
    void Start () {
        if(XRSettings.enabled)
        {
            return;
        }
        if (isAttached)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.position = cardLocation.transform.position;
            gameObject.transform.rotation = rotation;

            gameObject.transform.parent = cardLocation.transform;

        }
    }

    public override void GrabBegin(MouseGrabber grabber)
    {
        if (dormScript)
        {
            dormScript.grabbedID();
        }

        base.GrabBegin(grabber);
    }

    public override void GrabEnd(MouseGrabber grabber)
    {
        base.GrabEnd(grabber);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(MoveToPosition(gameObject, 1f));

        if (!isAttached)
        {
            isAttached = true;
            gameObject.transform.parent = cardLocation.transform;
        }

    }


    IEnumerator MoveToPosition(GameObject card, float time)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            card.transform.position = Vector3.Lerp(card.transform.position, cardLocation.transform.position, elapsedTime / time);
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, rotation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            //Debug.Log("CardRotation: " + card.transform.rotation.ToString());
            yield return new WaitForEndOfFrame();
        }

    }

}
