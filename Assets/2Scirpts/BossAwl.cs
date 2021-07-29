using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAwl : Bullet
{
    void Awake()
    {

    }

    void Update()
    {
        Destroy(gameObject, 5f);
    }

}