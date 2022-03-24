using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimAtPlayerTarget : MonoBehaviour
{
    public GameObject aimAt;
    public float rotateSpeed = 100;
    public float Range = 50f;
    public GameObject targets;
    public Camera cam;
    bool targetPlayer = false;

    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit)) targetPoint = hit.point;
        else targetPoint = ray.GetPoint(75);
        if (GameManager.instance.timeManager.GetComponent<timeManager>().isPaused || targetPlayer) return;
        aimAt.transform.position = Vector3.Lerp(aimAt.transform.position, targetPoint, Time.deltaTime * rotateSpeed);
    }
}
