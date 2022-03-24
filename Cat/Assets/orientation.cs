using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientation : MonoBehaviour
{
    public GameObject orientationObject;
 
    void Update()
    {
        transform.rotation = new Quaternion(orientationObject.transform.rotation.x, transform.rotation.y, orientationObject.transform.rotation.z, orientationObject.transform.rotation.w);
    }
}
