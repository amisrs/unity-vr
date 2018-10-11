using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CardGrabbable : OVRGrabbable {

    [SerializeField]
    private GameObject cardLocation;

    [SerializeField]
    private DormScript dormScript;
    [SerializeField]
    private bool isAttached = false;

    public bool isReturning = false;
    
    private Quaternion rotation = Quaternion.Euler(80.08701f, 94.552f, 69.333f);



    private void Start()
    {
        isCardGrabbable = false;
        soundEnabled = false;
        if(!XRSettings.enabled)
        {
            return;
        }

        if(isAttached)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //StartCoroutine(MoveToPosition(gameObject, 0.1f));
            gameObject.transform.position = cardLocation.transform.position;
            
            gameObject.transform.parent = cardLocation.transform;

        }
    }

    private void FixedUpdate()
    {
        if(isAttached && !isReturning && !isGrabbed)
        {
            gameObject.transform.position = cardLocation.transform.position;
            gameObject.transform.rotation = rotation;
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        if(dormScript)
        {
            dormScript.grabbedID();
            Debug.Log("Card grabbed");
        }
        base.GrabBegin(hand, grabPoint);

    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //move card to belt position
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(MoveToPosition(gameObject, 1f));

        if(!isAttached)
        {
            isAttached = true;
            gameObject.transform.parent = cardLocation.transform;
        }

    }

    IEnumerator MoveToPosition(GameObject card, float time)
    {
        float elapsedTime = 0.0f;
        isCardGrabbable = true;
        isReturning = true;
        while(elapsedTime < time)
        {
            card.transform.position = Vector3.Lerp(card.transform.position, cardLocation.transform.position, elapsedTime/time);
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, rotation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            //Debug.Log("CardRotation: " + card.transform.rotation.ToString());
            yield return new WaitForEndOfFrame();
        }
        isReturning = false;
        isCardGrabbable = false;
    }
}
