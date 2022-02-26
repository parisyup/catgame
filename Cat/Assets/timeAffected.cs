using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeAffected : MonoBehaviour
{
    Rigidbody body;
    float direction, magnitude;
    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }


    public void stopTime()
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.isKinematic = true;
    }
}
