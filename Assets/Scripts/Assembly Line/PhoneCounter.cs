﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneCounter : MonoBehaviour {

    [SerializeField]
    private FactoryScript factoryScript;

    [SerializeField]
    private bool isMainLine = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11 && isMainLine) //screweditem layer
        {
            ScrewSlotController screwSlot = other.gameObject.GetComponentInChildren<ScrewSlotController>();

            if(screwSlot.isScrewScrewed())
            {
                factoryScript.countPhone(true);
                //detect a good ipad
            } else {
                factoryScript.countPhone(false);
                //bad ipad
            }
        }
    }
}
