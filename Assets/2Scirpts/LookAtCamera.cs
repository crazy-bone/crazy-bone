using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera camera;

    void Update()
    {
        Vector3 v = camera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(camera.transform.position - v);
        transform.Rotate(0, 0, 0);
    }
}
