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

        Destroy(gameObject, 1f);
    }

    void ColliderSystem()
    {
        //Awl Damage¿Í °°À½
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

}
