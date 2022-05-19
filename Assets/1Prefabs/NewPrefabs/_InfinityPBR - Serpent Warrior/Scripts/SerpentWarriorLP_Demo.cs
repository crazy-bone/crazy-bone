using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentWarriorLP_Demo : MonoBehaviour
{
    
    private Animator animator;
    public Renderer[] rendererBody;
    public Renderer[] rendererArmor;
    public Renderer[] rendererLeather;
    public GameObject[] wardrobe;
    public int wardrobePercent = 60;
    public GameObject UI;
    
    // Start is called before the first frame update
    void Start () {
        animator = GetComponent<Animator> ();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SuperRandom();
        }
        if (Input.GetKeyDown(KeyCode.C))
            UI.SetActive(!UI.activeSelf);
    }

    public void Locomotion(float newValue){
        animator.SetFloat ("locomotion", newValue);
    }

    public void SuperRandom()
    {
        SetHueBody(Random.Range(0f, 1f));
        SetHueArmor(Random.Range(0f, 1f));
        SetHueLeather(Random.Range(0f, 1f));
        SetSaturationBody(Random.Range(-.4f, .4f));
        SetSaturationArmor(Random.Range(-.4f, .4f));
        SetSaturationLeather(Random.Range(-.4f, .4f));
        SetValueBody(Random.Range(-.2f, .2f));
        SetValueArmor(Random.Range(-.2f, .2f));
        SetValueLeather(Random.Range(-.2f, .2f));

        for (int i = 0; i < wardrobe.Length; i++)
        {
            wardrobe[i].SetActive(Random.Range(0, 100) < wardrobePercent);
        }
    }
    
    public void SetHueBody(float value)
    {
        for (int i = 0; i < rendererBody.Length; i++)
            rendererBody[i].material.SetFloat("_Hue", value);
    }
    
    public void SetSaturationBody(float value)
    {
        for (int i = 0; i < rendererBody.Length; i++)
            rendererBody[i].material.SetFloat("_Saturation", value);
    }
    
    public void SetValueBody(float value)
    {
        for (int i = 0; i < rendererBody.Length; i++)
            rendererBody[i].material.SetFloat("_Value", value);
    }
    
    public void SetHueArmor(float value)
    {
        for (int i = 0; i < rendererArmor.Length; i++)
            rendererArmor[i].material.SetFloat("_Hue", value);
    }
    
    public void SetSaturationArmor(float value)
    {
        for (int i = 0; i < rendererArmor.Length; i++)
            rendererArmor[i].material.SetFloat("_Saturation", value);
    }
    
    public void SetValueArmor(float value)
    {
        for (int i = 0; i < rendererArmor.Length; i++)
            rendererArmor[i].material.SetFloat("_Value", value);
    }
    
    public void SetHueLeather(float value)
    {
        for (int i = 0; i < rendererLeather.Length; i++)
            rendererLeather[i].material.SetFloat("_Hue", value);
    }
    
    public void SetSaturationLeather(float value)
    {
        for (int i = 0; i < rendererLeather.Length; i++)
            rendererLeather[i].material.SetFloat("_Saturation", value);
    }
    
    public void SetValueLeather(float value)
    {
        for (int i = 0; i < rendererLeather.Length; i++)
            rendererLeather[i].material.SetFloat("_Value", value);
    }
    
    
}
