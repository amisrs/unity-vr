using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ClockGrabbable : OVRGrabbable
{
    public GameObject scriptManager;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        Debug.Log("Clock grab begin");
        DormScript dormScript = scriptManager.GetComponent<DormScript>();
        dormScript.turnOffAlarm();

        hand.ForceRelease(this);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
    }
}
