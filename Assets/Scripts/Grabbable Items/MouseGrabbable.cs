using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrabbable : MonoBehaviour {
    // referenced from OVRGrabbable
    public bool soundEnabled = true;

    [SerializeField]
    public AudioClip grabSoundClip;
    public AudioSource grabSound;

    public float lastPlayedSound = 0.0f;
    public float soundGap = 0.05f;

    public AudioClip impactSoundClip;
   

    [SerializeField]
    public float rotationOffset = 0.0f;

    protected bool m_grabbedKinematic = false;
    protected bool m_useGravity = false;
    protected Collider m_grabbedCollider = null;
    protected float m_drag;
    public MouseGrabber m_grabbedBy = null;

    public bool isMovable = true;

    private void Awake()
    {
        grabSound = gameObject.GetComponent<AudioSource>();
        if (grabSound == null)
        {
            grabSound = gameObject.AddComponent<AudioSource>();
        }

        grabSound.playOnAwake = false;
        ONSPAudioSource oas = gameObject.GetComponent<ONSPAudioSource>();
        if(oas == null)
        {
            oas = gameObject.AddComponent<ONSPAudioSource>();
        }
        oas.UseInvSqr = true;

        grabSoundClip = Resources.Load("grab_sound") as AudioClip;
        impactSoundClip = Resources.Load("impact_sound") as AudioClip;

    }

    // Use this for initialization
    void Start () {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
        m_useGravity = GetComponent<Rigidbody>().useGravity;
        m_drag = GetComponent<Rigidbody>().drag;

    }

    // Update is called once per frame
    void Update () {
        lastPlayedSound += Time.deltaTime;
    }

    virtual public void GrabBegin(MouseGrabber grabber)
    {
        grabSound.PlayOneShot(grabSoundClip);
        m_grabbedBy = grabber;

        HingeJoint hj = gameObject.GetComponent<HingeJoint>();
        EnableGrabPhysics(gameObject.GetComponent<Rigidbody>());

        Physics.IgnoreCollision(this.GetComponent<Collider>(), grabber.transform.parent.GetComponent<Collider>());

        if (hj != null)
        {
            Rigidbody rb = hj.connectedBody;
            EnableGrabPhysics(rb);
            //rb.gameObject.GetComponent<MeshCollider>().enabled = false; // stop the screw from colliding

        }


    }

    virtual public void GrabEnd(MouseGrabber grabber)
    {
        m_grabbedBy = null;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        DisableGrabPhysics(rb);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), grabber.transform.parent.GetComponent<Collider>(), false);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (lastPlayedSound > soundGap && soundEnabled)
        {
            float relativeVelocity = collision.relativeVelocity.magnitude;
            ///Debug.Log(relativeVelocity);
            grabSound.PlayOneShot(impactSoundClip, relativeVelocity);
            lastPlayedSound = 0.0f;
        }
    }

    void EnableGrabPhysics(Rigidbody rb)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.drag = 0f;
    }

    void DisableGrabPhysics(Rigidbody rb)
    {
        Debug.Log("Disabling grab physics for: " + rb.name);
        rb.useGravity = m_useGravity;
        rb.isKinematic = m_grabbedKinematic;
        rb.drag = m_drag;
    }

}
