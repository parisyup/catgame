using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{
    public GameObject targets;
    public float rotateSpeed = 100;
    GameObject player;
    bool targetPlayer = false;
    void Start()
    {
        player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.timeManager.GetComponent<timeManager>().isPaused) return;
        targets.transform.position = Vector3.Lerp(targets.transform.position,player.transform.position, Time.deltaTime * rotateSpeed); 
    }

    public void isAttacking(bool isattack) { targetPlayer = isattack; }
}
