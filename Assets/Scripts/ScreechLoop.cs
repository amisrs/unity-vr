using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreechLoop : MonoBehaviour {

    AudioSource audioSource;
    

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartSoundLoop());
	}
	
    IEnumerator StartSoundLoop()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        yield return new WaitForSeconds(Random.Range(120.0f, 180.0f));
        StartCoroutine(StartSoundLoop());
    }

}
