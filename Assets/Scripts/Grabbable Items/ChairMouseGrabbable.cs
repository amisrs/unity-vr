using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairMouseGrabbable : MouseGrabbable {
    public GameObject scriptManager;
    private void Start()
    {
        isMovable = false;
    }

    public override void GrabBegin(MouseGrabber grabber)
    {
        //base.GrabBegin(grabber);
        Debug.Log("Chair grab begin");
        FactoryScript factoryScript = scriptManager.GetComponent<FactoryScript>();
        factoryScript.sitDown();

        grabber.GrabEnd();
    }

}
