using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedAwl : Bullet
{
    Animator anim;

    void Update()
    {
        

    }
    void OnCollisionEnter(Collision collision)
    {
        // 
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("DoAttack");

            Destroy(gameObject, 5f);

        }
    }
}
