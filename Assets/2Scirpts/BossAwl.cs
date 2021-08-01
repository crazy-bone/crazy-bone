using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAwl : Bullet
{


    void Update()
    {

        Destroy(gameObject, 5f);
    }

}