using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcVisualizer : MonoBehaviour {
	[Tooltip("Raycaster to visualize")]
	public ArcRaycaster arcRaycaster;
	[Tooltip("Line renderer used for visualization")]
	public LineRenderer arcRenderer;
	[Tooltip("Game object indicating when the raycaster hit something")]
	public Transform contactIndicator;
	[Tooltip("Game object indicating direction of raycast")]
	public Transform directionIndicator;

	[Tooltip("How many segments to use for curve, must be at least 3. More segments = better quality")]
	public int segments = 20;

	protected bool EarlyOut() {
		if (arcRenderer != null) {
			arcRenderer.enabled = HasController;
		}

		if (arcRaycaster == null || !HasController) {
			if (contactIndicator != null) {
				contactIndicator.gameObject.SetActive (false);
			}
			if (directionIndicator != null) {
				directionIndicator.gameObject.SetActive (false);
			}
			return true;
		}
		return false;
	}

	protected void SetCurveVisuals() {
		Color curveColor = Color.cyan;
		curveColor.a = arcRaycaster.MakingContact ? 1.0f : 0.25f;
        if(!arcRaycaster.MakingContact)
        {
            curveColor.r = 80.0f;
            curveColor.b = 80.0f;
            curveColor.g = 80.0f;
        }
		arcRenderer.startColor = curveColor;

		if (contactIndicator != null && arcRaycaster != null) {
            contactIndicator.gameObject.SetActive (arcRaycaster.MakingContact);
            //contactIndicator.gameObject.SetActive(true);
            if (arcRaycaster.MakingContact) { // change this to make indicator not show
				contactIndicator.position = arcRaycaster.HitPoint;
				contactIndicator.rotation = Quaternion.LookRotation (Vector3.forward, arcRaycaster.Normal);
			}
		}

		if (directionIndicator != null && arcRaycaster != null) {
            directionIndicator.gameObject.SetActive (arcRaycaster.MakingContact && TouchpadTouched);
            //contactIndicator.gameObject.SetActive(true);
            if ((arcRaycaster.MakingContact) && TouchpadTouched) { 
				directionIndicator.position = arcRaycaster.HitPoint;
				directionIndicator.rotation = Quaternion.LookRotation (TouchpadDirection, arcRaycaster.Normal);
			}
		}
	}

	bool TouchpadTouched {
		get {
			return OVRInput.Get(OVRInput.Touch.PrimaryTouchpad);
		}
	}

	OVRInput.Controller Controller {
		get {
            return OVRInput.Controller.LTrackedRemote;

            OVRInput.Controller controllers = OVRInput.GetConnectedControllers ();
			if ((controllers & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote) {
                Debug.Log("Has left controller");
                return OVRInput.Controller.LTrackedRemote;
			}
			if ((controllers & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote) {
                Debug.Log("Has right controller");
                return OVRInput.Controller.RTrackedRemote;
			}
			return OVRInput.Controller.None;
		}
	}

	Matrix4x4 ControllerToWorldMatrix {
		get {
			if (!HasController) {
				return Matrix4x4.identity;
			}

			Matrix4x4 localToWorld = arcRaycaster.trackingSpace.localToWorldMatrix;

			Quaternion orientation = OVRInput.GetLocalControllerRotation(Controller);
			Vector3 position = OVRInput.GetLocalControllerPosition (Controller);

			Matrix4x4 local = Matrix4x4.TRS (position, orientation, Vector3.one);

			Matrix4x4 world = local * localToWorld;

			return world;
		}
	}

	Vector3 TouchpadDirection {
		get {
			Vector2 touch = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
			Vector3 forward = new Vector3 (touch.x, 0.0f, touch.y).normalized;
			forward = ControllerToWorldMatrix.MultiplyVector (forward);
			forward = Vector3.ProjectOnPlane (forward, Vector3.up);
			return forward.normalized;
		}
	}

	bool HasController {
        
		get {
            return true;
			OVRInput.Controller controllers = OVRInput.GetConnectedControllers ();
			if ((controllers & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote) {
				return true;
			}
			if ((controllers & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote) {
				return true;
			}
            Debug.Log("does not have controller");
            return false;

		}
	}
}
