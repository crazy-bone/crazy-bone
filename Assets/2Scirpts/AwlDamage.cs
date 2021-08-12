using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwlDamage : Bullet
{
    private void Awake()
    {
        Invoke("ColliderSystem", 0);
    }
    void Update()
    {
        
        Destroy(gameObject, 1f);
    }

    void ColliderSystem()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

}
