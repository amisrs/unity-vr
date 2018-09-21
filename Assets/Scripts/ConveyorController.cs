using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Vector3 speedVector;
    [SerializeField]
    private float direction; // y-axis rotation (for now assume 0 all the time please)
    [SerializeField]
    private float length;
    [SerializeField]
    private float width;
    [SerializeField]
    private float thickness;

    [SerializeField]
    private bool isRunning;

    [SerializeField]
    private float offset;
    [SerializeField]
    private float textureSpeed;



    private int numberOfBlocks;
    private Vector3 startOfQueue;
    private Renderer renderer;
	// Use this for initialization
	void Start () {
        isRunning = false;
        numberOfBlocks = 5;
        speedVector = new Vector3(speed, 0f, 0f);
        offset = 0f;
        textureSpeed = 0.05f;
        Vector3 scale = new Vector3(length, thickness, width);
        transform.localScale = scale;

        renderer = GetComponent<Renderer>();
        //startOfQueue = new Vector3((transform.position.x + 0.5f * length) - 2.5f * length, transform.position.y, transform.position.z);

        /*for (int i = 0; i < numberOfBlocks; i++)
        {
            Vector3 newPosition = new Vector3((transform.position.x + 0.5f * length + i*length) - 2.5f*length, transform.position.y, transform.position.z);
            GameObject conveyorBlock = Instantiate(conveyorBlockPrefab, newPosition, Quaternion.Euler(0.0f, direction, 0.0f), transform);
            conveyorBlocks.Add(conveyorBlock);
        } */
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isRunning && collision.gameObject.CompareTag("Conveyable")) {
            collision.gameObject.transform.Translate(speedVector, Space.World);
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(speedVector*10, ForceMode.Acceleration);
        }
        
    }

    public void toggleRunning()
    {
        isRunning = !isRunning;
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        if(isRunning)
        {
            renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0f));
            offset -= textureSpeed;
        }
        if(offset <= -1f)
        {
            offset = 0;
        }
	}
}
