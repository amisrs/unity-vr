
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryWorkerController : MonoBehaviour {


    public float randomFloat;
    public float nextRandomFloat;

    private Animator animator;
    private int randomFloatHash;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        randomFloatHash = Animator.StringToHash("randomFloat");
        nextRandomFloat = GetRandomFloat();
        StartCoroutine("DelayAnimation");
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle")) {
            animator.SetFloat(randomFloatHash, nextRandomFloat);
            nextRandomFloat = GetRandomFloat();
        }

    }
    private float GetRandomFloat()
    {
        return Random.value;
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 3.0f));
        animator.SetBool(Animator.StringToHash("isEnabled"), true);
    }
}
