using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private float interval;
    [SerializeField]
    private GameObject item;

    private float timeCounter;

    [SerializeField]
    private bool isRunning;

    [SerializeField]
    private int spawnLimit = 30;
    private int spawnCount = 0;

    [SerializeField]
    private FactoryScript factoryScript;

	// Use this for initialization
	void Start () {
        timeCounter = interval;	
	}
	
	// Update is called once per frame
	void Update () {
        if(isRunning)
        {
            timeCounter += Time.deltaTime;
            //Debug.Log("yo");

            if (timeCounter >= interval)
            {
                //Debug.Log("Spawning object.");
                Instantiate(item, transform.position, transform.rotation, transform.parent);
                spawnCount++;
                timeCounter = 0;

                if(spawnCount >= spawnLimit)
                {
                    factoryScript.stopSpawning();
                }
            }
        }
	}

    public void toggleRunning()
    {
        isRunning = !isRunning;
    }

    public void setSpawnLimit(int limit)
    {
        spawnLimit = limit;
    }

    public int getSpawnLimit()
    {
        return spawnLimit;
    }
}
