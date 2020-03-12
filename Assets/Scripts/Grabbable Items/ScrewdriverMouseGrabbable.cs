using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverMouseGrabbable : MouseGrabbable {

    [SerializeField]
    FactoryScript factoryScript;

    // Use this for initialization
    public override void GrabBegin(MouseGrabber grabber)
    {
        base.GrabBegin(grabber);
        factoryScript.ScrewdriverGrabbed();
    }

    public override void GrabEnd(MouseGrabber grabber)
    {
        base.GrabEnd(grabber);
    }
}
