using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour {
    public DormScript dormScript;
    public OVRScreenFade screenFade;
    private float fadeTime = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().name == "dorm")
            {
                FadeAndLoad(1);
            } else if (SceneManager.GetActiveScene().name == "entrance")
            {
                Debug.Log("Loading the factory");
                FadeAndLoad(2);
            } else if (SceneManager.GetActiveScene().name == "factory")
            {
                FadeAndLoad(3);
            }
            
        }
    }

    public void ForceExit()
    {
        FadeAndLoad(3);
    }

    void FadeAndLoad(int scene)
    {
        screenFade.FadeOut(fadeTime);
        StartCoroutine(WaitAndLoad(scene));
//        float elapsedTime = 0.0f;

//        while (elapsedTime < fadeTime)
//        {
//            elapsedTime += Time.deltaTime;
//
//            if(elapsedTime >= fadeTime)
//            {
//                SceneManager.LoadScene(scene);
//                break;
//            }
//        }
    }

    IEnumerator WaitAndLoad(int scene)
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}
