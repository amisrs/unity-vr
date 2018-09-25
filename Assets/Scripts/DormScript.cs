using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DormScript : MonoBehaviour {
    public enum GameStage
    {
        ALARMING,
        GRABBING,
        LEAVING
    }

    [SerializeField]
    GameObject clock;
    [SerializeField]
    GameObject door;
    [SerializeField]
    GameObject card;

    [SerializeField]
    public GameStage gameStage;

    [SerializeField]
    public GameObject GrabToTurnOff;
    [SerializeField]
    public GameObject TurnOffAlarm;
    [SerializeField]
    public GameObject GrabID;

    [SerializeField]
    public GameObject clockAudio;
    [SerializeField]
    public GameObject doorAudio;

    bool alarmOff = false;
    bool idGrabbed = false;


    // Use this for initialization
    void Start () {
        gameStage = GameStage.ALARMING;
    }

    public void turnOffAlarm()
    {
        clockAudio.SetActive(false);
        gameStage = GameStage.GRABBING;
        //GrabToTurnOff.GetComponentInChildren<TextMeshPro>().SetText("Go out the door");
        TurnOffAlarm.SetActive(false);
        GrabToTurnOff.SetActive(false);

        GrabID.SetActive(true);
        alarmOff = true;
        // check ide grabbed
        if(idGrabbed)
        {
            proceed();
        }

    }

    public void grabbedID()
    {
        Debug.Log("ID grabbed, opening door");
        GrabID.SetActive(false);

        idGrabbed = true;
        if(alarmOff)
        {
            proceed();
        }
    }

    void proceed()
    {
        doorAudio.GetComponent<AudioSource>().Play();
        door.GetComponent<DoorGrabbable>().UnlockDoor();
    }

    // turn off alarm
    // unlock door
    // leave

    // Update is called once per frame
    void Update () {
		
	}
}
