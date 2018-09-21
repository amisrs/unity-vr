using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverGrabbable : OVRGrabbable {

    [SerializeField]
    FactoryScript factoryScript;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        Debug.Log("Chair grab begin");
        factoryScript.ScrewdriverGrabbed();
       
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
    }
}
