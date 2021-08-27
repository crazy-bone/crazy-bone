using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public Transform target;
    public float moveSpeed;
    public float turnSpeed;
    public float destroyTime;

    void Update()
    {
        // 일정시간 후 소멸
        if (destroyTime > 0f)
            Destroy(gameObject, destroyTime);

        if (target != null)
        {
            Vector3 targetPosition = target.position + new Vector3(0f, 5f, 0f);
            Vector3 direction = targetPosition - transform.position;

            //transform.rotation.SetLookRotation(target.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
}
