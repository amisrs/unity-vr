using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapZoneController : MonoBehaviour {
    [SerializeField]
    private GateController gateController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Card"))
        {
            gateController.openGate();
        }
    }
}
