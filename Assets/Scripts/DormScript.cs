using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
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

    [SerializeField]
    public TextMeshProUGUI instruction;
    [SerializeField]
    public TextMeshProUGUI instructionVR;

    private string instruction1 = "Turn off the alarm";
    private string instruction2 = "Grab your ID";
    private string instruction3 = "Exit the dorms";

    bool alarmOff = false;
    bool idGrabbed = false;


    // Use this for initialization
    void Start () {
        gameStage = GameStage.ALARMING;
        if(XRSettings.enabled)
        {
            instructionVR.SetText(instruction1);
        } else
        {
            instruction.SetText(instruction1);
        }
    }

    public void turnOffAlarm()
    {
        clockAudio.SetActive(false);
        gameStage = GameStage.GRABBING;
        //GrabToTurnOff.GetComponentInChildren<TextMeshPro>().SetText("Go out the door");
        TurnOffAlarm.SetActive(false);
        GrabToTurnOff.SetActive(false);

        
        alarmOff = true;
        // check ide grabbed
        if(idGrabbed)
        {
            proceed();
        } else
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction2);
            }
            else
            {
                instruction.SetText(instruction2);
            }
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
        } else
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction1);
            }
            else
            {
                instruction.SetText(instruction1);
            }
        }
    }

    void proceed()
    {
        if(gameStage != GameStage.LEAVING)
        {
            gameStage = GameStage.LEAVING;
        } else
        {
            return;
        }
        

        if (XRSettings.enabled)
        {
            instructionVR.SetText(instruction3);
        }
        else
        {
            instruction.SetText(instruction3
                );
        }
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
