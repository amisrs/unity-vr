using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewSlotController : MonoBehaviour {
    [SerializeField]
    private GameObject tick;

    public GameObject screw;

    private ScrewController screwController;

    [SerializeField]
    private FirstPhone firstPhone;

    [SerializeField]
    private FactoryScript factoryScript;
	// Use this for initialization
	void Start () {
        factoryScript = GameObject.Find("scriptManager").GetComponent<FactoryScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ScrewSlot: detected a collider " + other.gameObject.name + " entering trigger.");
        if (other.gameObject.CompareTag("Screw") && screw == null && other.gameObject.layer != 10)
        {
            screwController = other.gameObject.GetComponent<ScrewController>();
            screwController.placeScrew(gameObject);
            screw = other.gameObject;
            GetComponent<Renderer>().enabled = false;
            
            if(firstPhone != null)
            {
                firstPhone.ScrewPlaced();
            }
            
        }
    }

    public bool isScrewScrewed()
    {
        if(screwController == null)
        {
            return false;
        } else
        {
            return screwController.screwState == ScrewController.ScrewState.SCREWED;
        }
        
    }

    public void displayTick()
    {
        transform.parent.Find("Tick").gameObject.SetActive(true);
        if (firstPhone != null)
        {
            firstPhone.ScrewTurned();
        } else
        {
            factoryScript.screwedPhone();
        }

    }
}
