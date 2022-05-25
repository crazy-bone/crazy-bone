using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;
    bool istrigger = false;
    public void OnTriggerEnter(Collider other)
    {
        if(istrigger == false)
        Trigger();
    }
    public void Trigger()
    {
        istrigger = true;
        var system = FindObjectOfType<DialogueSystem>();
        system.Begin(info);
    }
    public void ChangeTrigger()
    {
        istrigger = false;

        Destroy(gameObject);
    }
   
}
