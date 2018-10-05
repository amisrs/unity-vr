using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class FactoryScript : MonoBehaviour
{

    public enum GameStage
    {
        BEGIN,
        SITTING,
        WORKING,
        LEAVING
    }

    [SerializeField]
    public GameObject playerVR;
    [SerializeField]
    public GameObject player;


    [SerializeField]
    GameObject chair;

    [SerializeField]
    GameObject phone;

    [SerializeField]
    public GameStage gameStage;

    private Vector3 chairOffset = new Vector3(0.1f, 0.3f, 0.6f);

    [SerializeField]
    private Vector3 chairOffsetVR = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    private ConveyorController[] conveyorControllers;
    private Spawner[] spawners;

    [SerializeField]
    private float workTimer = 0.0f;
    [SerializeField]
    private float maxWorkTime = 600.0f;
    private bool isTimerOn = false;

    [SerializeField]
    private TextMeshPro goodPhonesText;
    //[SerializeField]
    //private TextMeshPro badPhonesText;
    [SerializeField]
    private TextMeshPro screwedPhonesText;



    private int screwedPhoneCount = 0;

    [SerializeField]
    private int badPhoneCount = 0;
    [SerializeField]
    private int goodPhoneCount = 0;

    private int spawnLimit = 30;
    private string goodPhoneLabel = "Good phones: {0}";
    private string badPhoneLabel = "Bad phones: {0}";
    private string screwedPhoneLabel = "{0}/";

    private string instruction1 = "Grab your chair";
    private string instruction2 = "Grab a screw";
    private string instruction3 = "Place the screw";
    private string instruction4 = "Grab the screwdriver";
    private string instruction5 = "Turn the screw";
    private string instruction6 = "Place phone on belt";
    private string instruction7 = "Repeat.";

    [SerializeField]
    private GameObject GrabYourChair;
    [SerializeField]
    private GameObject GrabAScrew;
    [SerializeField]
    private GameObject PlaceTheScrew;
    [SerializeField]
    private GameObject GrabScrewdriver;
    [SerializeField]
    private GameObject TurnScrew;
    [SerializeField]
    private GameObject PlaceOnBelt;

    [SerializeField]
    private GameObject conveyorAudio;
    [SerializeField]
    private GameObject phoneDingAudio;

    [SerializeField]
    private ExitTrigger exitTrigger;

    [SerializeField]
    private TextMeshProUGUI instruction;

    [SerializeField]
    private TextMeshProUGUI instructionVR;

    [SerializeField]
    private GameObject teleporter;
    [SerializeField]
    private GameObject chairPosition;

    private int TutorialStage = 0;

    private bool isShiftHeld = false;


    // Use this for initialization
    void Start()
    {
        
        conveyorControllers = FindObjectsOfType<ConveyorController>();
        spawners = FindObjectsOfType<Spawner>();
        screwedPhoneLabel = "{0}/" + spawnLimit;
        gameStage = GameStage.BEGIN;
        if(XRSettings.enabled)
        {
            instructionVR.SetText(instruction1);
            //playerVR.transform.LookAt(chair.gameObject.transform);
        } else
        {
            instruction.SetText(instruction1);
            //player.transform.LookAt(chair.gameObject.transform);
        }
        
    }

    public void sitDown()
    {
        if (gameStage == GameStage.BEGIN)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction2);
            }
            else
            {
                instruction.SetText(instruction2);
            }
            // disable teleportation
            gameStage = GameStage.SITTING;

            if (XRSettings.enabled)
            {
                playerVR.transform.position = chair.transform.position;// + chairOffsetVR;

                OVRPlayerController oVRPlayerController = playerVR.GetComponent<OVRPlayerController>();
                oVRPlayerController.SitDown();
                oVRPlayerController.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                player.transform.position = chair.transform.position + chairOffset;

                FirstPersonController firstPersonController = player.GetComponent<FirstPersonController>();
                firstPersonController.SitDown();
                firstPersonController.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            teleporter.SetActive(false);
            GrabYourChair.SetActive(false);
            GrabAScrew.SetActive(true);
            Debug.Log("sit yourself");
        }
    }

    public void ScrewGrabbed()
    {
        if (TutorialStage == 0)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction3);
            }
            else
            {
                instruction.SetText(instruction3);
            }

            GrabAScrew.SetActive(false);
            PlaceTheScrew.SetActive(true);
            TutorialStage = 1;
        }
    }

    public void FirstScrewPlaced()
    {
        if (TutorialStage == 1)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction4);
            }
            else
            {
                instruction.SetText(instruction4);
            }

            PlaceTheScrew.SetActive(false);
            GrabScrewdriver.SetActive(true);
        }
        TutorialStage = 2;
    }

    public void ScrewdriverGrabbed()
    {
        if (TutorialStage == 2)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction5);
            }
            else
            {
                instruction.SetText(instruction5);
            }

            GrabScrewdriver.SetActive(false);
            TurnScrew.SetActive(true);
        }
        TutorialStage = 3;
    }

    public void ScrewTurned()
    {
        if (TutorialStage == 3)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction6);
            }
            else
            {
                instruction.SetText(instruction6);
            }

            //phoneDingAudio.GetComponent<AudioSource>().Play();
            TurnScrew.SetActive(false);
            PlaceOnBelt.SetActive(true);
        }
        TutorialStage = 4;
    }
    //detect screw grabbed
    //detect screw placed
    //detect screwdriver grabbed
    //detect screwed

    public void startWork()
    {
        if (gameStage == GameStage.SITTING)
        {
            if (XRSettings.enabled)
            {
                instructionVR.SetText(instruction7);
            }
            else
            {
                instruction.SetText(instruction7);
            }

            StartCoroutine(StartLastMessage(PlaceOnBelt, 10.0f));
            //PlaceOnBelt.SetActive(false);
            gameStage = GameStage.WORKING;
            foreach (ConveyorController conveyor in conveyorControllers)
            {
                conveyor.toggleRunning();
            }
            conveyorAudio.SetActive(true);

            foreach (Spawner spawner in spawners)
            {
                spawner.setSpawnLimit(spawnLimit);
                spawner.toggleRunning();
            }
            isTimerOn = true;
            // turn on conveyors and spawners
        }
    }
    public void screwedPhone()
    {
        screwedPhoneCount++;
        screwedPhonesText.SetText(screwedPhoneLabel, screwedPhoneCount);
        phoneDingAudio.GetComponent<AudioSource>().Play();
        Stats.Goodphones += 1;

    }
    public void countPhone(bool isGood)
    {
        if (isGood)
        {
            Debug.Log("Counted good phone.");
            goodPhoneCount++;
            goodPhonesText.SetText(goodPhoneLabel, goodPhoneCount);

        }
        else
        {
            Debug.Log("Counted bad phone.");
            badPhoneCount++;
            Stats.Badphones += 1;
            //badPhonesText.SetText(badPhoneLabel, badPhoneCount);
        }

        if (goodPhoneCount + badPhoneCount >= spawnLimit)
        {
            // all the phones are done.
        }
    }

    public void stopSpawning()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.toggleRunning();
        }
        standUp();
        finishWork();


    }

    public void finishWork()
    {
        Debug.Log("Moving trigger");
        exitTrigger.ForceExit(4);
    }

    public void standUp()
    {
        gameStage = GameStage.LEAVING;
        if (XRSettings.enabled)
        {
            OVRPlayerController oVRPlayerController = playerVR.GetComponent<OVRPlayerController>();
            oVRPlayerController.StandUp();

        }
        else
        {
            FirstPersonController firstPersonController = player.GetComponent<FirstPersonController>();
            firstPersonController.StandUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {
            workTimer += Time.deltaTime;

            if (workTimer >= maxWorkTime)
            {
                isTimerOn = false;
                stopSpawning();
            }
        }

        if(Input.GetKey(KeyCode.LeftShift)) {
            isShiftHeld = true;
        } else
        {
            isShiftHeld = false;
        }

        if(isShiftHeld && Input.GetKeyDown(KeyCode.F5))
        {
            workTimer = 0.25f * maxWorkTime;
        } else if (Input.GetKeyDown(KeyCode.F6))
        {
            workTimer = 0.5f * maxWorkTime;
        } else if (Input.GetKeyDown(KeyCode.F7))
        {
            workTimer = 0.75f * maxWorkTime;
        } else if (Input.GetKeyDown(KeyCode.F8))
        {
            workTimer = 0.99f * maxWorkTime;
        }

        //playerVR.transform.position = chair.transform.position + chairOffsetVR;
    }

    IEnumerator StartLastMessage(GameObject plate, float time)
    {
        plate.GetComponentInChildren<TextMeshPro>().fontSize = 0.2f;
        plate.GetComponentInChildren<TextMeshPro>().SetText("More phones are on the way");
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        plate.SetActive(false);
    }
}
