using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockMouseGrabbable : MouseGrabbable {
    public GameObject scriptManager;

    public override void GrabBegin(MouseGrabber grabber)
    {
        DormScript dormScript = scriptManager.GetComponent<DormScript>();
        dormScript.turnOffAlarm();

        grabber.GrabEnd();
    }


}
