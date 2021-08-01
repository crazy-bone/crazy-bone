using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwlDamage2 : Bullet
{
    private void Awake()
    {
        Invoke("ColliderSystem", 2);
    }
    void Update()
    {

        Destroy(gameObject, 5f);
    }

    void ColliderSystem()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

}
