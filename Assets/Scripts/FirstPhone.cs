using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPhone : MonoBehaviour {

    [SerializeField]
    FactoryScript factoryScript;

    private ScrewSlotController screwSlot;
    private bool isStarted;

	// Use this for initialization
	void Start () {
        isStarted = false;
        screwSlot = GetComponentInChildren<ScrewSlotController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ScrewPlaced()
    {
        factoryScript.FirstScrewPlaced();
    }

    public void ScrewTurned()
    {
        factoryScript.ScrewTurned();
        factoryScript.screwedPhone();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isStarted && collision.gameObject.layer == 9 && screwSlot.isScrewScrewed()) // conveyorlayer
        {
            factoryScript.startWork();
        }
            
    }
}
