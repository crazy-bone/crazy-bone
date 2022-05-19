using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_MonsterCreature2Demo : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void Locomotion(float value)
    {
        animator.SetFloat("Locomotion", value);
    }

    public void Turning(float value)
    {
        animator.SetFloat("Turning", value);
    }

    public void TriggerAnimation(string value){
        animator.SetTrigger(value);
    }

}
