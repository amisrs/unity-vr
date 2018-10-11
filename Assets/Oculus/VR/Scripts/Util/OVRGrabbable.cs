/************************************************************************************

Copyright   :   Copyright 2017 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.4.1 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// An object that can be grabbed and thrown by OVRGrabber.
/// </summary>
/// 
// Modified by Gavin:
// - Made something public, can't remember what.
// - Removed automatic kinematicification of grabbed object.
// - Added no gravity to grabbed object.

// WARNING: be careful when using grabbable hingejoint!!


public class OVRGrabbable : MonoBehaviour
{
    [SerializeField]
    protected bool m_allowOffhandGrab = true;
    [SerializeField]
    protected bool m_snapPosition = false;
    [SerializeField]
    protected bool m_snapOrientation = false;
    [SerializeField]
    protected Transform m_snapOffset;
    [SerializeField]
    protected Collider[] m_grabPoints = null;
    [SerializeField]
    public AudioClip grabSoundClip;
    public AudioSource grabSound;

    public float lastPlayedSound = 0.0f;
    public float soundGap = 0.05f;

    public AudioClip impactSoundClip;

    public bool soundEnabled = true;
    protected bool m_grabbedKinematic = false;
    protected bool m_useGravity = false;
    protected Collider m_grabbedCollider = null;
    protected float m_drag;
    protected OVRGrabber m_grabbedBy = null;

    public bool isCardGrabbable = false;

	/// <summary>
	/// If true, the object can currently be grabbed.
	/// </summary>
    public bool allowOffhandGrab
    {
        get { return m_allowOffhandGrab; }
    }

	/// <summary>
	/// If true, the object is currently grabbed.
	/// </summary>
    public bool isGrabbed
    {
        get { return m_grabbedBy != null; }
    }

	/// <summary>
	/// If true, the object's position will snap to match snapOffset when grabbed.
	/// </summary>
    public bool snapPosition
    {
        get { return m_snapPosition; }
    }

	/// <summary>
	/// If true, the object's orientation will snap to match snapOffset when grabbed.
	/// </summary>
    public bool snapOrientation
    {
        get { return m_snapOrientation; }
    }

	/// <summary>
	/// An offset relative to the OVRGrabber where this object can snap when grabbed.
	/// </summary>
    public Transform snapOffset
    {
        get { return m_snapOffset; }
    }

	/// <summary>
	/// Returns the OVRGrabber currently grabbing this object.
	/// </summary>
    public OVRGrabber grabbedBy
    {
        get { return m_grabbedBy; }
    }

	/// <summary>
	/// The transform at which this object was grabbed.
	/// </summary>
    public Transform grabbedTransform
    {
        get { return m_grabbedCollider.transform; }
    }

	/// <summary>
	/// The Rigidbody of the collider that was used to grab this object.
	/// </summary>
    public Rigidbody grabbedRigidbody
    {
        get { return m_grabbedCollider.attachedRigidbody; }
    }

	/// <summary>
	/// The contact point(s) where the object was grabbed.
	/// </summary>
    public Collider[] grabPoints
    {
        get { return m_grabPoints; }
    }

	/// <summary>
	/// Notifies the object that it has been grabbed.
	/// </summary>
	virtual public void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        grabSound.PlayOneShot(grabSoundClip);
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
        //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        EnableGrabPhysics(gameObject.GetComponent<Rigidbody>());

        // need to get the rbs from configurable as well
        HingeJoint hj = gameObject.GetComponent<HingeJoint>();

        if (hj != null)
        {
            Rigidbody rb = hj.connectedBody;
            EnableGrabPhysics(rb);
            //rb.gameObject.GetComponent<MeshCollider>().enabled = false; // stop the screw from colliding

        }        
    }

    void EnableGrabPhysics(Rigidbody rb)
    {
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.drag = 0f;
    }

    void DisableGrabPhysics(Rigidbody rb)
    {
        rb.useGravity = m_useGravity;
        rb.isKinematic = m_grabbedKinematic;
        rb.drag = m_drag;
    }

    /// <summary>
    /// Notifies the object that it has been released.
    /// </summary>
    virtual public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        DisableGrabPhysics(rb);
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;

        HingeJoint hj = gameObject.GetComponent<HingeJoint>();
        if(hj != null)
        {
            Rigidbody rbHingeJoint = hj.connectedBody;
            if (rbHingeJoint != null)
            {
                DisableGrabPhysics(rbHingeJoint);
                rbHingeJoint.gameObject.gameObject.GetComponent<MeshCollider>().enabled = true;
                rbHingeJoint.velocity = linearVelocity;
                rbHingeJoint.angularVelocity = angularVelocity;
            }
        }
        
        

        m_grabbedBy = null;
        m_grabbedCollider = null;
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

    private void Update()
    {
        lastPlayedSound += Time.deltaTime;
    }

    void Awake()
    {
        grabSound = gameObject.GetComponent<AudioSource>();
        if(grabSound == null)
        {
            grabSound = gameObject.AddComponent<AudioSource>();
        }
        grabSound.playOnAwake = false;
        ONSPAudioSource oas = gameObject.AddComponent<ONSPAudioSource>();
        oas.UseInvSqr = true;

        grabSoundClip = Resources.Load("grab_sound") as AudioClip;
        impactSoundClip = Resources.Load("impact_sound") as AudioClip;
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
				throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
    }

    protected virtual void Start()
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
        m_useGravity = GetComponent<Rigidbody>().useGravity;
        m_drag = GetComponent<Rigidbody>().drag;
    }

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }
}
