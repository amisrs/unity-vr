using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://answers.unity.com/questions/29741/mouse-look-script.html

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{

    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;

    // multiplier of input values
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    // this is used in ClampAngle
    public float minX = -360f;
    public float maxX = 360f;
    public float minY = -60f;
    public float maxY = 60f;


    float rotationX = 0f;
    float rotationY = 0f;

    // this is set at the beginning of the script
    Quaternion originalRotation;

    // Use this for initialization
    void Start()
    {

        //  if (rigidbody) {
        //      rigidbody.freezeRotation = true;
        //  }

        originalRotation = transform.localRotation;
    }

    // stop angle from going over 360 or below 0
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }

        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    // Update is called once per frame
    void Update()
    {
        // three cases
        // the mouse moves in both axes
        // the mouse moves in only x
        // the mouse moves in only y

        if (axes == RotationAxes.MouseXAndY)
        {
            transform.localRotation = originalRotation * RotateX() * RotateY();

        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.localRotation = originalRotation * RotateX();
        }
        else if (axes == RotationAxes.MouseY)
        {
            transform.localRotation = originalRotation * RotateY();
        }
    }

    Quaternion RotateX()
    {
        // set rotation based on axis movement
        float xDelta = Input.GetAxis("Mouse X") * sensitivityX;
        Debug.Log("xDelta: " + xDelta);
        rotationX += xDelta;
        // then we clampangle it
        rotationX = ClampAngle(rotationX, minX, maxX);
        // then we convert to quaternion
        Quaternion quaternionX = Quaternion.AngleAxis(rotationX, Vector3.up);
        // then we apply the quaternion to the transform's rotation 
        Debug.Log("Rotating: " + quaternionX.ToString());
        return quaternionX;
        transform.localRotation = originalRotation * quaternionX;

    }

    Quaternion RotateY()
    {
        // set rotation based on axis movement
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        // then we clampangle it
        rotationY = ClampAngle(rotationY, minY, maxY);
        // then we convert to quaternion
        Quaternion quaternionY = Quaternion.AngleAxis(rotationY, -Vector3.right);
        // then we apply the quaternion to the transform's rotation 
        return quaternionY;
        transform.localRotation = originalRotation * quaternionY;


    }
}
