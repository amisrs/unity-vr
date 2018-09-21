using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryScript : MonoBehaviour {

   public enum GameStage
    {
        BEGIN,
        SITTING,
        WORKING,
        LEAVING
    }

    [SerializeField]
    public GameObject player;

    [SerializeField]
    GameObject chair;

    [SerializeField]
    GameObject phone;

    [SerializeField]
    public GameStage gameStage;

    private Vector3 chairOffset = new Vector3(0.1f, 0.0f, 0.3f);
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

    private int TutorialStage = 0;


    // Use this for initialization
    void Start()
    {
        conveyorControllers = FindObjectsOfType<ConveyorController>();
        spawners = FindObjectsOfType<Spawner>();
        screwedPhoneLabel = "{0}/" + spawnLimit;
        gameStage = GameStage.BEGIN;


    }

    public void sitDown()
    {
        if(gameStage == GameStage.BEGIN)
        {
            // disable teleportation
            gameStage = GameStage.SITTING;
            player.transform.position = chair.GetComponent<Renderer>().bounds.center + chairOffset;
            OVRPlayerController oVRPlayerController = player.GetComponent<OVRPlayerController>();
            oVRPlayerController.SitDown();
            oVRPlayerController.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            GrabYourChair.SetActive(false);
            GrabAScrew.SetActive(true);
            Debug.Log("sit yourself");
        }
    }

    public void ScrewGrabbed()
    {
        if(TutorialStage == 0)
        {
            GrabAScrew.SetActive(false);
            PlaceTheScrew.SetActive(true);
            TutorialStage = 1;
        }
    }

    public void FirstScrewPlaced()
    {
        if(TutorialStage == 1)
        {
            PlaceTheScrew.SetActive(false);
            GrabScrewdriver.SetActive(true);
        }
        TutorialStage = 2;
    }

    public void ScrewdriverGrabbed()
    {
        if(TutorialStage == 2)
        {
            GrabScrewdriver.SetActive(false);
            TurnScrew.SetActive(true);
        }
        TutorialStage = 3;
    }

    public void ScrewTurned()
    {
        if(TutorialStage == 3)
        {
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
        if(gameStage == GameStage.SITTING)
        {
            PlaceOnBelt.SetActive(false);
            gameStage = GameStage.WORKING;
            foreach (ConveyorController conveyor in conveyorControllers)
            {
                conveyor.toggleRunning();
            }
            conveyorAudio.SetActive(true);

            foreach(Spawner spawner in spawners)
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

        } else
        {
            Debug.Log("Counted bad phone.");
            badPhoneCount++;
            Stats.Badphones += 1;
            //badPhonesText.SetText(badPhoneLabel, badPhoneCount);
        }

        if(goodPhoneCount + badPhoneCount >= spawnLimit)
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
        exitTrigger.ForceExit();
    }

    public void standUp()
    {
        gameStage = GameStage.LEAVING;
        OVRPlayerController oVRPlayerController = player.GetComponent<OVRPlayerController>();
        oVRPlayerController.StandUp();
    }

    // Update is called once per frame
    void Update () {
		if(isTimerOn)
        {
            workTimer += Time.deltaTime;

            if(workTimer >= maxWorkTime)
            {
                isTimerOn = false;
                stopSpawning();
            }
        }
	}
}
