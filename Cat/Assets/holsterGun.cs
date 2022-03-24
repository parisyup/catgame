using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holsterGun : MonoBehaviour
{
    public GameObject barrel;
    public GameObject rotater;
    public GameObject rig;
    public bool holster = false;
    public float timer = 3;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) holster = true;
        if (holster)
        {
            //rig.active = false;
            barrel.transform.rotation = Quaternion.Slerp(barrel.transform.rotation, new Quaternion(90, barrel.transform.rotation.y, barrel.transform.rotation.z, barrel.transform.rotation.w), Time.deltaTime);
            rotater.transform.rotation = Quaternion.Slerp(rotater.transform.rotation, new Quaternion(90, rotater.transform.rotation.y, rotater.transform.rotation.z, rotater.transform.rotation.w), Time.deltaTime);
        }
    }
}
