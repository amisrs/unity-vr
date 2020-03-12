using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverController : MonoBehaviour {

    // increase when:
    // screwdriver rotates clockwise along the local z axis
    //      AND screwdriver is grabbed
    //      AND screwdriver head is on screw head
    //
    // decrease when:
    // screwdriver rotates anticlockwise along the local z axis
    //      OR the screwdriver is let go
    //      
    enum ScrewdriverState
    {
        DEFAULT,
        GRABBED,
        ENGAGED
    }

    [SerializeField]
    private GameObject screwdriverLocation;

    Quaternion previousTwistRotation;
    Quaternion twistRotation;
    ScrewdriverState screwdriverState = ScrewdriverState.DEFAULT;
    GameObject screw;
    //List<GameObject> screws = new List<GameObject>();
    ScrewController screwController;

    // Use this for initialization
    void Start () {
        previousTwistRotation = Quaternion.Euler(0f, 0f, 0f);
        twistRotation = Quaternion.Euler(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Vector3 angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        //Debug.Log("Screwdriver angular velocity: " + angularVelocity);

        twistRotation = transform.localRotation;
        Quaternion rotationDelta = Quaternion.Inverse(previousTwistRotation) * twistRotation;
        //if(Mathf.Abs(previousTwistRotation.eulerAngles.z - twistRotation.eulerAngles.z))
        if(rotationDelta.eulerAngles.z >= 180)
        {
            //they turned it clockwise
            
        }
        else if(screwdriverState == ScrewdriverState.ENGAGED && rotationDelta.eulerAngles.z > 1)
        {
            screwController.screwIn(rotationDelta);
            //Debug.Log(rotationDelta.eulerAngles.z);
        }
        
        // rotationDelta = 



        // at the very end
        previousTwistRotation = transform.localRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ScrewheadTrigger") && screwdriverState != ScrewdriverState.ENGAGED)
        {
            Debug.Log("Screwdriver engaged.");
            screwdriverState = ScrewdriverState.ENGAGED;
            //screws.Add(other.gameObject);
            screw = other.transform.parent.gameObject;
            screwController = screw.GetComponent<ScrewController>();
            screwController.setScrewStateScrewing(this.gameObject);
            //screwController.setScrewState(ScrewController.ScrewState.SCREWING);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("ScrewheadTrigger") && screwdriverState == ScrewdriverState.ENGAGED)
        {
            // screwdriver left the screwhead area, don't screw
            screwdriverState = ScrewdriverState.GRABBED;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // return screwdriver to desk
        if(collision.gameObject.layer == 14) // floor layer
        {
            StartCoroutine(MoveToPosition(gameObject, 1.0f));
        }
    }

    IEnumerator MoveToPosition(GameObject screwdriver, float time)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            screwdriver.transform.position = Vector3.Lerp(screwdriver.transform.position, screwdriverLocation.transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }



}
