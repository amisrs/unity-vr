using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGrabbable : MonoBehaviour
{
    public GameObject scriptManager;
    private bool isLocked = true;

    //https://forum.unity.com/threads/door-open-close-in-c.410811/
    //public Transform door;
    public float endRotation;
    public float startRotation;
    public float speed;
    IEnumerator OpenDoor()
    {
        for (int r = 0; r < speed; r += 1)
        {
            transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(transform.localEulerAngles.y, endRotation, 5f / speed), 0);
            yield return null;
        }
    }

    public void UnlockDoor()
    {
        Debug.Log("Unlocking door!!");
        isLocked = false;
        StartCoroutine("OpenDoor");   
    }

}
