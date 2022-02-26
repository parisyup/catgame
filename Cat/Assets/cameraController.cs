using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    void Start()
    {
    }

    void Update()
    {
        camera.transform.position = player.transform.position;
    }
}
