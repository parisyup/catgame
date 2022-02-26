using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public playerController playerScript;
    public AdrenalineBarController adreanlineBar;
    public SpeedOMeterController speedController;
    float currentSpeed;

    public void Update()
    {
        speedController.updateCurrentSpeed(currentSpeed);

    }
    private void Start()
    {
        speedController.setMaxSpeed(playerScript.momontumSpeed);
        playerScript = GetComponent<playerController>();
        adreanlineBar.setAdreanlineValues(playerScript.momentumTimerMax, 0f);
    }

    public void updateAdreanline(float currentAdreanline)
    {
        adreanlineBar.setAdrenaline(currentAdreanline);
    }

    public void updateCurrentSpeed(float Speed)
    {
        currentSpeed = Speed;
    }
}
