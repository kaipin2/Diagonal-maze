using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Slime : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3(0, 1f, 0);
    }
}
