using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger1 : MonoBehaviour
{
    public Dialogue info;
    bool istrigger = false;
   public GameObject GameObject;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

            if (istrigger == false)
                Trigger();
        }
    }
    public void Trigger()
    {
        istrigger = true;
        var system = FindObjectOfType<DialogueSystem>();
        system.Begin(info);
    }
    public void ChangeTrigger()
    {
        Debug.Log("d");
        istrigger = false;
        GameObject.SetActive(true);
        Destroy(gameObject);
    }
   
}
