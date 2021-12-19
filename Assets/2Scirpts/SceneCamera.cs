using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    public enum CameraMode
    {
        IDLE,
        FollowTarget,
        Moving
    }

    public CameraMode mode;
    public Transform target;
    public Vector3 offset;

    public int num = 1;

    private Vector3 destination;
    private float speed;

    void Update()
    {
        

        if (mode == CameraMode.FollowTarget) // Player을 따라가는 카메라 설정

            transform.position = target.position + offset;
        else if (mode == CameraMode.Moving)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) < 0.01)
            {
                mode = CameraMode.IDLE;
            }
        }
        if(Boss.attackPhase == 1 && num == 1)
        {
            Debug.Log("페이즈 1");
            StartCoroutine(Move1());

        }
        if (Boss.attackPhase == 2 && num == 2)
        {
            Debug.Log("페이즈 2");
            StartCoroutine(Move2());

        }
        if (Boss.attackPhase == 3 && num == 3)
        {
            Debug.Log("페이즈 3");
            StartCoroutine(Move3());

        }
        if (Boss.attackPhase == 4 && num == 4)
        {
            Debug.Log("페이즈 4");
            StartCoroutine(Move4());

        }
        if (Boss.attackPhase == 5 && num == 5)
        {
            Debug.Log("페이즈 5");
            StartCoroutine(Move5());

        }
    }
    
    public void MoveTo(Vector3 destination, float speed)
    {
        mode = CameraMode.Moving;
        this.destination = destination + offset;
        this.speed = speed;
    }
    IEnumerator Player()
    {
        transform.position = target.position + offset;
        yield return null;
       
    }


    IEnumerator Move1()
    {
        this.transform.position = new Vector3(0, 50, -128);
        yield return new WaitForSeconds(2f);
        if (num == 1)
        {
            num++;
        }
        yield break;
    }
    IEnumerator Move2()
    {
        this.transform.position = new Vector3(0, 50, -128);
        yield return new WaitForSeconds(2f);
        if (num == 2)
        {
            num++;
        }
        yield break;
    }
    IEnumerator Move3()
    {
        this.transform.position = new Vector3(0, 50, -128);
        yield return new WaitForSeconds(2f);
        if (num == 3)
        {
            num++;
        }
        yield break;
    }
    IEnumerator Move4()
    {
        this.transform.position = new Vector3(0, 50, -128);
        yield return new WaitForSeconds(2f);
        if (num == 4)
        {
            num++;
        }
        yield break;
    }
    IEnumerator Move5()
    {
        this.transform.position = new Vector3(0, 50, -128);
        yield return new WaitForSeconds(2f);
        if (num == 5)
        {
            num++;
        }
        yield break;
    }
}
