using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Icon made by Smashicons from www.flaticon.com 


public class MouseGrabber : MonoBehaviour {

    // when you grab something, where is it in relation to the grabber?
    [SerializeField]
    public float distanceFromFace = 0.4f;
    private float maxDistanceFromFace = 1.4f;
    private float minDistanceFromFace = 0.3f;
    private float defaultDistanceFromFace = 0.4f;

    private float rotationOffset;

    [SerializeField]
    private Image crosshairImage;
    [SerializeField]
    private Sprite defaultCrosshair;
    [SerializeField]
    private Sprite handCrosshair;

    private Camera camera;

    [SerializeField]
    private float maxGrabDistance = 1.6f;

    [SerializeField]
    private GameObject grabCandidate;
    [SerializeField]
    private GameObject grabbedObject;

    private bool isGrabbing = false;
    [SerializeField]
    private bool isInitialMovement = true;

    private bool isRotating = false;
    [SerializeField]
    private float rotationSpeed = 4.0f;
    private float totalRotation = 0.0f;

    public GameObject attachedObject;

    Ray ray;
    int layerMask;
    

    bool m_useGravity;
    bool m_grabbedKinematic;
    float m_drag;

    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();
        int layerMaskIgnoreRaycast = 1 << 2;
        //layerMaskIgnoreRaycast = ~layerMaskIgnoreRaycast;
        int layerMaskTelenope = 1 << 16;
        //layerMaskTelenope = ~layerMaskTelenope;

        layerMask = layerMaskIgnoreRaycast | layerMaskTelenope;
        layerMask = ~layerMask;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("input detected");
            if(grabCandidate && !isGrabbing)
            {
                Debug.Log("starting grab!");
                GrabBegin();
            } else if (isGrabbing)
            {
                GrabEnd();
            }
        }

        isRotating = Input.GetMouseButton(1);

        float distanceChangeRaw = Input.GetAxis("Mouse ScrollWheel");
        float distanceChange = 0.0f;

        distanceChange = distanceChangeRaw * 0.3f;
        if (distanceChange != 0)
        {
            Debug.Log("Scrolled: " + distanceChange);
        }

        if (distanceChange < 0)
        {
            if (distanceChange + distanceFromFace >= minDistanceFromFace)
            {
                distanceFromFace += distanceChange;
            } else
            {
                distanceFromFace = minDistanceFromFace;
            }
        } else
        {
            if (distanceChange + distanceFromFace <= maxDistanceFromFace)
            {
                distanceFromFace += distanceChange;
            } else
            {
                distanceFromFace = maxDistanceFromFace;
            }
        }


        //distanceFromFace += Input.GetAxis("Mouse ScrollWheel");

        ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(ray.origin, Vector3.ClampMagnitude(ray.direction, maxGrabDistance));
        RaycastHit raycastHit;
        if(Physics.Raycast(ray, out raycastHit, maxGrabDistance, layerMask))
        {
            if(raycastHit.collider.gameObject)
            {
                if (raycastHit.collider.gameObject.GetComponent<MouseGrabbable>())
                {
                    crosshairImage.sprite = handCrosshair;
                    grabCandidate = raycastHit.collider.gameObject;
                } else
                {
                    crosshairImage.sprite = defaultCrosshair;
                    grabCandidate = null;
                }
            } else
            {
                crosshairImage.sprite = defaultCrosshair;
                grabCandidate = null;
            }
            Debug.Log("Looking at: " + raycastHit.collider.gameObject.name);
        } else
        {
            crosshairImage.sprite = defaultCrosshair;
            grabCandidate = null;
        }
    }

    public void GrabBegin()
    {
        isGrabbing = true;
        crosshairImage.enabled = false;

        grabbedObject = grabCandidate;

        MouseGrabbable mouseGrabbable = grabbedObject.GetComponent<MouseGrabbable>();
        mouseGrabbable.GrabBegin(this);

        HingeJoint hj = grabbedObject.GetComponent<HingeJoint>();
        if (hj != null)
        {
            //Debug.Log("There is a hinge joint!");
            Rigidbody rbHingeJoint = hj.connectedBody;
            if (rbHingeJoint != null)
            {
                //Debug.Log("And theres a " + rbHingeJoint.name + " attacheD!!");
                m_useGravity = rbHingeJoint.useGravity;
                m_grabbedKinematic = rbHingeJoint.isKinematic;
                m_drag = rbHingeJoint.drag;
                EnableGrabPhysics(rbHingeJoint);
                attachedObject = rbHingeJoint.gameObject;
                //rbHingeJoint.gameObject.gameObject.GetComponent<MeshCollider>().enabled = true;
            }
        }

        //if its a clock or chair don't do this...
        if (mouseGrabbable.isMovable)
        {
            rotationOffset = mouseGrabbable.rotationOffset;
            StartCoroutine(MoveToGrabber(0.2f));
        }
    }

    public void GrabEnd()
    {
        isGrabbing = false;
        crosshairImage.enabled = true;

        MouseGrabbable mouseGrabbable = grabbedObject.GetComponent<MouseGrabbable>();
        mouseGrabbable.GrabEnd(this);

        rotationOffset = 0.0f;
        distanceFromFace = defaultDistanceFromFace;
        isInitialMovement = true;
        totalRotation = 0.0f;

        HingeJoint hj = grabbedObject.GetComponent<HingeJoint>();
        if (hj != null)
        {
            Rigidbody rbHingeJoint = hj.connectedBody;
            if (rbHingeJoint != null)
            {
                Debug.Log("Disabling grab physics for: " + rbHingeJoint.name);
                DisableGrabPhysics(rbHingeJoint);
                attachedObject = null;
                rbHingeJoint.gameObject.gameObject.GetComponent<MeshCollider>().enabled = true;
            }
        }

        // stop the grabbable following
        // restore physics
    }

    private void FixedUpdate()
    {
        // move grabbed object
        if(isGrabbing && grabbedObject && !isInitialMovement)
        {
            if(attachedObject != null)
            {
               // attachedObject.transform.position = grabbedObject.transform.position;
               // attachedObject.transform.rotation = grabbedObject.transform.rotation;

            }
            grabbedObject.transform.position = transform.position + ray.direction.normalized * distanceFromFace;

            grabbedObject.transform.rotation = 
                transform.rotation * Quaternion.Euler(rotationOffset, 0.0f, 0.0f)
                * Quaternion.Euler(0.0f, 0.0f, totalRotation);

            if(isRotating)
            {
                Debug.Log("is rotating");
                grabbedObject.transform.rotation =
                    grabbedObject.transform.rotation * Quaternion.Euler(0.0f, 0.0f, rotationSpeed);

                totalRotation += rotationSpeed;
            }
        }
    }

    IEnumerator MoveToGrabber(float time)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            grabbedObject.transform.position =
                Vector3.Lerp(grabbedObject.transform.position,
                transform.position + ray.direction.normalized * distanceFromFace, elapsedTime / time);
            grabbedObject.transform.rotation =
                Quaternion.Lerp(grabbedObject.transform.rotation,
                transform.rotation * Quaternion.Euler(rotationOffset, 0.0f, 0.0f), elapsedTime / time);
            yield return new WaitForEndOfFrame();
        }
        isInitialMovement = false;
    }

    void EnableGrabPhysics(Rigidbody rb)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.drag = 0f;
    }

    void DisableGrabPhysics(Rigidbody rb)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.drag = m_drag;
    }

}
