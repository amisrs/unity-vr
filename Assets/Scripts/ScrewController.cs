using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ScrewController : MonoBehaviour
{
    public enum ScrewState
    {
        OPEN,
        PLACED,
        SCREWING,
        SCREWED
    }

    public Quaternion placedRotation;

    [SerializeField]
    public ScrewState screwState;

    private OVRGrabbable grabbableComponentVR;
    private MouseGrabbable grabbableComponent;
    private OVRGrabber grabberVR;
    private MouseGrabber grabber;
    private HingeJoint hingeJoint;
    private float depthPerDegree;
    private GameObject screwSlot;
    private float goalRotation = 14400.0f;
    private float currentRotation = 0.0f;

    [SerializeField]
    private AudioSource screwSound;
    [SerializeField]
    private AudioClip screwSoundClip;

    // Use this for initialization
    void Start()
    {
        screwState = ScrewState.OPEN;
        placedRotation = Quaternion.Euler(0f, 0f, 0f);
        grabbableComponentVR = GetComponent<OVRGrabbable>();
        grabbableComponent = GetComponent<MouseGrabbable>();
        depthPerDegree = 0.0125f;
        screwSound = gameObject.GetComponent<AudioSource>();
        if (screwSound == null)
        {
            screwSound = gameObject.AddComponent<AudioSource>();
        }
        screwSound.playOnAwake = false;
        ONSPAudioSource oas = gameObject.AddComponent<ONSPAudioSource>();
        oas.UseInvSqr = true;
        screwSoundClip = Resources.Load("screw_sound") as AudioClip;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void placeScrew(GameObject slot)
    {
        if (screwState == ScrewState.OPEN)
        {
            setScrewState(ScrewState.PLACED);
            Debug.Log("get placed son");
            screwSlot = slot;
            becomeScrewed(slot);
            toggleArrows(true);
        }

    }

    public void becomeScrewed(GameObject slot)
    {
        gameObject.layer = 10; //PlacedScrew layer, which ignores collision with ScrewedItem layer

        if (XRSettings.enabled)
        {
            grabberVR = grabbableComponentVR.grabbedBy;
            try
            {
                grabberVR.ForceRelease(grabbableComponentVR);
            }
            catch
            {
                Debug.Log("Tried to force grabber to release. It's probably NPE (next line will try print what it tried to release).");
                Debug.Log("Tried to force release: " + grabbableComponent.gameObject.name);
            }

        }
        else
        {
            grabber = grabbableComponent.m_grabbedBy;
            try
            {
                grabber.GrabEnd();
            }
            catch
            {
                Debug.Log("Tried to force grabber to release. It's probably NPE (next line will try print what it tried to release).");
                Debug.Log("Tried to force release: " + grabbableComponent.gameObject.name);
            }

        }

        gameObject.transform.SetPositionAndRotation(slot.transform.position, slot.transform.rotation * placedRotation);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 0;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        // turn meshcollider into a trigger so it doesn't get knocked by screwdriver
        // i tried making it kinematic but it stops rotation that way

        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        //meshCollider.isTrigger = true;

        Debug.Log("Adding hingejoint");
        hingeJoint = slot.transform.parent.gameObject.AddComponent<HingeJoint>();
        hingeJoint.anchor = Vector3.zero;
        hingeJoint.axis = Vector3.forward;
        hingeJoint.connectedAnchor = Vector3.zero;


        hingeJoint.connectedBody = gameObject.GetComponent<Rigidbody>();

        Destroy(grabbableComponent);
        Destroy(grabbableComponentVR);
        //grabbableComponent.enabled = false;
    }

    // when the screwdriver trigger enters the screwhead trigger, listen for screwdriver rotation
    // every 1 degree clockwise, do screwIn
    public void setScrewStateScrewing(GameObject screwdriver)
    {
        if (screwState == ScrewState.PLACED)
        {
            hingeJoint.autoConfigureConnectedAnchor = false;
            screwState = ScrewState.SCREWING;
            Debug.Log("start screwing son");
            //screwdriver.GetComponent<ScrewdriverController>().listenforrotation

        }
    }

    // rotate clockwise and sink in
    public void screwIn(Quaternion screwRotation)
    {
        if (screwState == ScrewState.SCREWING)
        {
            //Debug.Log("Gonna rotate screw: " + screwRotation.eulerAngles.z);

            gameObject.transform.Rotate(0f, 0f, -screwRotation.eulerAngles.z * 80f, Space.Self);
            currentRotation += screwRotation.eulerAngles.z * 80f;
            //Debug.Log("Also shoving the screw in z axis: " + depthPerDegree * screwRotation.eulerAngles.magnitude);
            hingeJoint.anchor += new Vector3(0f, 0f, depthPerDegree * (screwRotation.eulerAngles.magnitude / 150) * 40);

            // play noise
            screwSound.PlayOneShot(screwSoundClip);

            if (hingeJoint.anchor.z >= 30.0f || currentRotation >= goalRotation)
            {
                // finished screwing
                Debug.Log("Finished screwing.");
                setScrewState(ScrewState.SCREWED);
                screwSlot.GetComponent<ScrewSlotController>().displayTick();
                toggleArrows(false);
                //hingeJoint.anchor = new Vector3(0f, 0f, 0.9f);
                //hingeJoint.autoConfigureConnectedAnchor = true;
                //gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //gameObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
        else
        {
            Debug.Log("Trying to screw screwed screw.");
        }
    }



    public void setScrewState(ScrewState screwState)
    {
        this.screwState = screwState;
    }

    public void toggleArrows(bool value)
    {
        transform.Find("Arrows").gameObject.SetActive(value);
    }


}
