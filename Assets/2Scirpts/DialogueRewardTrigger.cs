using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueRewardTrigger : MonoBehaviour
{
    public GameObject Ammor;
    public GameObject Monster;
    public GameObject MonsterB;
    public AudioSource audio;
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
        Destroy(gameObject);

        audio.Play();
        Ammor.SetActive(true);
        Monster.SetActive(true);
        MonsterB.SetActive(true);
    }
   
}
