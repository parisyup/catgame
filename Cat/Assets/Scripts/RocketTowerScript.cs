using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTowerScript : MonoBehaviour
{
    public GameObject launchPosition; // sets the position where the rocket will spawn
    public GameObject Rocket; // setting the rocket
    public float shotsPerSecond = 0.2f; //fire rate
    float timeToFire = 0f; // fire rate timer

    timeManager timeManager;
    void Start()
    {
        timeManager = GameManager.instance.timeManager.GetComponent<timeManager>();
        Rocket = GameManager.instance.rocket; // gets the rocket from the gameManager
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.isPaused) return;
        bool readyToFire = timeToFire <= 0; // checks if its ready to fire
        if (readyToFire) // fires if ready
        {
            GameObject CurrentRocket = Instantiate(Rocket, launchPosition.transform.position, Quaternion.Euler(transform.up)); // creates a rocket
            CurrentRocket.SetActive(true); // activates the rocket
            timeToFire = 1f / shotsPerSecond; // sets the time
        }
        if(timeToFire > 0) // decrease time if its above 0
        {
            timeToFire -= Time.deltaTime;
        }
        else if(timeToFire < 0) // sets the time to fire to 0 so it doesnt go negative 
        {
            timeToFire = 0f;
        }
    }
}
