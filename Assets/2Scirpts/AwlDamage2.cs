using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwlDamage2 : Bullet
{
    private void Awake()
    {
        Invoke("ColliderSystem", 0);
    }
    void Update()
    {

        Destroy(gameObject, 3f);
    }

    void ColliderSystem()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

}
