using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixBody : MonoBehaviour
{
    Vector3 lastHit;
    void Update()
    {
        RaycastHit hit; //simple raycast downwards
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            
            if (lastHit == hit.normal) return;
            transform.parent.parent.up = hit.normal;
            lastHit = hit.normal;
        }

    }
}
