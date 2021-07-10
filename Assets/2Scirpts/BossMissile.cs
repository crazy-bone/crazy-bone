using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public Transform target;
    NavMeshAgent nav;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

    }

    void Update()
    {

        Destroy(gameObject, 5f);
        nav.SetDestination(target.position);
    }
}
