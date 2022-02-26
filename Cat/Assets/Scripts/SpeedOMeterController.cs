using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedOMeterController : MonoBehaviour
{
    public float currentSpeed;
    public float maxSpeed;
    public string Speed;
    Text speedText;

    private void Start()
    {
        speedText = GetComponent<Text>();
    }
    void Update()
    {
        double currentSpeedv2;
        currentSpeedv2 = System.Math.Round(currentSpeed, 2);
        if (currentSpeed > 42f)
        {
            Speed = "Max 42 ";
        }
        else
        {
            Speed = currentSpeedv2.ToString();
        }
        speedText.text = "Speed : " + Speed + " m/s";
    }

    public void updateCurrentSpeed(float currentVelocity)
    {
        currentSpeed = currentVelocity;
    }

    public void setMaxSpeed(float max)
    {
        maxSpeed = max;
    }
}
