using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public Transform target;
    public float speed;
    public float destroyTime;

    void Update()
    {
        // 5ÃÊ ÈÄ ¼Ò¸ê
        if (destroyTime > 0f)
            Destroy(gameObject, destroyTime);

        if (target != null)
        {
            Vector3 targetPosition = target.position + new Vector3(0f, 5f, 0f);
            transform.rotation.SetLookRotation(target.position);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); ;
        }
    }
}
