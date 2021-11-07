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
    }
    
    public void MoveTo(Vector3 destination, float speed)
    {
        mode = CameraMode.Moving;
        this.destination = destination + offset;
        this.speed = speed;
    }
}
