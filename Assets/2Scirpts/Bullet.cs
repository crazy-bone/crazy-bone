using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void Start()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            // 총알이 벽을 맞을 시 제거
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            // 총알이 벽을 맞을 시 제거
            Destroy(gameObject);
        }
    }

    public void DamageUp()
    {
        damage += 100;
    }
}
