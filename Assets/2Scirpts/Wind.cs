using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Wind : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;

    void OnTriggerStay()  // WindZone Trigger
    {
        Rigidbody bulletRigid = GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * -500;
        // 트리거 안에 있는동안 계속 적용
    }
}
