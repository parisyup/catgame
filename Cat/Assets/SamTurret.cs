using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamTurret : MonoBehaviour
{
    public GameObject aimAt;
    public float rotateSpeed = 100;
    public float Range = 50f;
    public GameObject[] targets;
    bool targetPlayer = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Missile");
        if (GameManager.instance.timeManager.GetComponent<timeManager>().isPaused || targetPlayer) return;
        if (targets.Length == 0) { return; }
        aimAt.transform.position = Vector3.Lerp(aimAt.transform.position, targets[0].transform.position, Time.deltaTime * rotateSpeed);
    }

    //public void isAttacking(bool isattack) { targetPlayer = isattack; }
}
