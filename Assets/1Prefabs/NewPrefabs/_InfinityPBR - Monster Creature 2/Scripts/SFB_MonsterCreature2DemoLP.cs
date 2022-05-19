using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_MonsterCreature2DemoLP : MonoBehaviour {

    public Animator animator;
    public Renderer[] renderer;
    public GameObject[] randomObjects;
    
    // Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Randomize();
        }
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

    public void SetHue(float value)
    {
        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetFloat("_Hue", value);
        }
    }
    
    public void SetSaturation(float value)
    {
        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetFloat("_Saturation", value);
        }
    }
    
    public void SetValue(float value)
    {
        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetFloat("_Value", value);
        }
    }

    public void Randomize()
    {
        SetHue(Random.Range(0f,1f));
        for (int i = 0; i < randomObjects.Length; i++)
        {
            randomObjects[i].SetActive(GetRandom());
        }
    }

    public bool GetRandom()
    {
        if (Random.Range(0, 2) == 0)
            return false;

        return true;
    }

}
