using System.Collections;
using System.Collections.Generic;
//using InfinityPBR;
using UnityEngine;

public class MonsterCreature1LP_Demo : MonoBehaviour
{
    private Animator animator;
    public GameObject[] materials;
    public Renderer[] renderer;
    public Renderer[] rendererGear;
	
    //public BlendShapesManager[] bsmanager;
    public GameObject canvas;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
    public void Locomotion(float newValue){
        animator.SetFloat ("Locomotion", newValue);
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Randomize();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCanvas();
        }
    }
	
    /*
    public void Randomize()
    {
        foreach (var t in bsmanager)
        {
            foreach (var bs in t.blendShapeGameObjects)
            {
                foreach (var bsv in bs.blendShapeValues)
                {
                    t.SetRandomShapeValue(bsv);
                }
            }
        }
        SetHue(Random.Range(0f,1f));
        SetSaturation(Random.Range(-.4f,.0f));
        SetValue(Random.Range(-.2f,.2f));
        
        SetHueGear(Random.Range(0f,1f));
        SetSaturationGear(Random.Range(-.2f,.2f));
        SetValueGear(Random.Range(-.2f,.2f));
    }
    */
	
    public void ToggleCanvas()
    {
        canvas.SetActive(!canvas.active);
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
    
    public void SetHueGear(float value)
    {
        for (int i = 0; i < rendererGear.Length; i++)
        {
            rendererGear[i].material.SetFloat("_Hue", value);
        }
    }
    
    public void SetSaturationGear(float value)
    {
        for (int i = 0; i < rendererGear.Length; i++)
        {
            rendererGear[i].material.SetFloat("_Saturation", value);
        }
    }
    
    public void SetValueGear(float value)
    {
        for (int i = 0; i < rendererGear.Length; i++)
        {
            rendererGear[i].material.SetFloat("_Value", value);
        }
    }
}
