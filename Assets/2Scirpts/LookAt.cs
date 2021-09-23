using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // 시점 방향으로 이동하는 스크립트

    public Player cam; //메인카메라
    private float speed = 0.5f; // 이동속도

    void Start()
    {
    }

    void Update()
    {
        MoveLookAt();
    }
    void MoveLookAt()
    {
        //메인카메라가 바라보는 방향
        Vector3 dir = cam.transform.localRotation * Vector3.forward;
        //카메라가 바라보는 방향으로 바라보게
        transform.localRotation = cam.transform.localRotation;
        //Rotation.x값을 freeze해놓았지만 움직여서 따로 Rotation값을 0으로 세팅.
        transform.localRotation = new Quaternion(0, transform.localRotation.y, 0, transform.localRotation.w);
        //바라보는 시점 방향으로 이동.
        gameObject.transform.Translate(dir * 0.1f * Time.deltaTime);
    }
}
