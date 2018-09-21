using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewBox : MonoBehaviour
{

    [SerializeField]
    List<GameObject> screwsInBox = new List<GameObject>();

    [SerializeField]
    GameObject screwPrefab;

    [SerializeField]
    FactoryScript factoryScript;

    private bool isFirstScrew = true;

    private int defaultScrewNumber = 5;

    // Use this for initialization
    void Start()
    {
        for(int i=0; i<defaultScrewNumber; i++)
        {
            CreateScrew();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Screw") && !other.isTrigger)
        if (other.gameObject.CompareTag("ScrewheadTrigger") && other.isTrigger)
        {
            // they put a screw in the box
            Debug.Log("A screw was put in the box." + other.gameObject.name);
            screwsInBox.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Screw") && !other.isTrigger)
        if (other.gameObject.CompareTag("ScrewheadTrigger") && other.isTrigger)
        {
            // they took the screw out
            Debug.Log("A screw was taken out of the box." + other.gameObject.name);
            screwsInBox.Remove(other.gameObject);

            //Destroy(other.gameObject);
            if (screwsInBox.Count < defaultScrewNumber)
            {
                Debug.Log("Running out of screws, make a new one.");
                CreateScrew();
            }

            if(isFirstScrew)
            {
                factoryScript.ScrewGrabbed();
                isFirstScrew = false;
            }

        }
    }

    public void CreateScrew()
    {
        Instantiate(screwPrefab, transform.position + new Vector3(Random.Range(0.0f, 0.01f), Random.Range(0.0f, 0.01f), Random.Range(0.0f, 0.01f)), transform.rotation, transform.parent.transform.parent);
    }
}
