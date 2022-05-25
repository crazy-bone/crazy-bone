using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    bool isA = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isA == false)
        {
            isA = true;
            a.SetActive(true);
            b.SetActive(true);

        }
    }
}
