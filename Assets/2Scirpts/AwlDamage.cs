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
        // Awl에 Triger 넣으니까 계속 밀리는 현상 해결하기 위해서 Awl 하위 오브젝트인 Empty에 범위만 넣고 Damage를 입힘 
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

}
