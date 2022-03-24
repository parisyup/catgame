using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public Animator anim;
    Rigidbody playerRB;
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.6f, player.transform.position.z);

        if (playerRB.velocity.magnitude >= 4f)
        {
            anim.SetFloat("speed", 1f, 0.15f, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("speed", 0f, 0.15f, Time.deltaTime);
        }
    }
}
