using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSetActive : MonoBehaviour
{
    public GameObject Door;
    public GameObject NPC;

    public void setActive()
    {
        Door.SetActive(true);
        NPC.SetActive(true);
    }
}
