using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairGrabbable : OVRGrabbable {

    
    public GameObject scriptManager;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        Debug.Log("Chair grab begin");
        FactoryScript factoryScript = scriptManager.GetComponent<FactoryScript>();
        factoryScript.sitDown();

        hand.ForceRelease(this);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
    }
}
