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
        // Floor과 Wall을 맞았을때 점프가 끝나야 하기 때문에 설정
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("DoAttack");

            Destroy(gameObject, 5f);

        }
    }
}
