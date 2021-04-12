using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{

    public Transform traget;
    public float orbitSpeed;
    Vector3 offSet;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(traget.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
