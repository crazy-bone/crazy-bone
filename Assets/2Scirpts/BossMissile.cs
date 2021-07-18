using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public Transform target;
    public float speed;

    void Update()
    {
        // 5ÃÊ ÈÄ ¼Ò¸ê
        Destroy(gameObject, 5f);

        if (target != null)
        {
            transform.rotation.SetLookRotation(target.position);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); ;
        }
    }
}
