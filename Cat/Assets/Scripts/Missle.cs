using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public GameManager gameManager; // game manager where (its just a script with global variables that u will always call)
    public timeManager timeManager; // game manager where (its just a script with global variables that u will always call)
    [Header("Setup")]
    public Transform RocketTarget; //setting the target for the rocket (u can have it set to random enemies with in a certain range)
    public Rigidbody RocketRgb; //the rockets rigid body (the object needs a rigid body to work)
    public float blastRadius = 6f; // blast radius for the rocket
    public float blastForce = 1000f; // blast force to add knock back
    public float damage = 10f; // damage
    public float enemyDamageMultiplier = 2f; //damage multiplier when hits spicific targets
    public float buildingDamageMultiplier = 3f; // damage multiplier when hits buildings

    public GameObject explosionEffect; // just an effect for an explosion made by unity's particle system

    public float turnSpeed = 1f; //turn speed for the rocket
    public float rocketFlySpeed = 13f; // rocket speed

    private Transform rocketLocalTrans; // local transform

    Collision collided;
    bool explode = false;

    
    void Start()
    {   //setting the variables
        rocketLocalTrans = GetComponent<Transform>(); 
        gameManager = GameManager.instance;
        timeManager = GameManager.instance.timeManager.GetComponent<timeManager>();
        RocketTarget = gameManager.Player.transform;
    }


    private void FixedUpdate()
    {
        if (timeManager.isPaused) return;
        /*        RaycastHit hit;
        if (Physics.Raycast(transform.position, -orientation.up, out hit, 3f))
        {
            Vector3.Lerp(transform.position, new Vector3 (transform.position.x, hit.transform.position.y + 1f, transform.position.z), 1f);
        }*/

        float RocketTargetY = RocketTarget.transform.position.y; //gets y target (up or down) so that the rocket doesnt get too close to the ground and blow up from touching it
        Vector3.Lerp(transform.position, new Vector3(transform.position.x, RocketTargetY, transform.position.z), Time.deltaTime * rocketFlySpeed); // move rocket to player using lerping
        Vector3 velocity = rocketLocalTrans.forward * rocketFlySpeed; // calculate rocket speed
        //velocity = new Vector3(velocity.x, 0f, velocity.z);
        if (transform.position.y <= 2f) RocketRgb.velocity = new Vector3(RocketRgb.velocity.x, 1f, RocketRgb.velocity.z); //fix hight so that the rocket doesnt touch the ground
        else RocketRgb.velocity = velocity; // sets the rocket speed
        
        var rocketTargetRot = Quaternion.LookRotation(RocketTarget.position - rocketLocalTrans.position); //Now Turn the Rocket towards the Target

        RocketRgb.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, rocketTargetRot, turnSpeed)); // rotate rocket

        if (explode) boom(collided);
    }

    private void OnCollisionEnter(Collision collision) // finds a collision
    {
        explode = true;
        collided = collision;
        if (timeManager.isPaused) return;
        boom(collision);
    }

    public void boom(Collision collision)
    {
        Destroy(gameObject); // destroy the rocket
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.Euler(transform.up)); // do the explosion
        Destroy(explosion, 2f); // destroy the explosion after 2 seconds
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius); // gets every collider within the blast radius 

        foreach (Collider hitObject in colliders) // for loop for each collider 
        {
            Rigidbody rigidBody = hitObject.GetComponent<Rigidbody>(); // gets the rigidbody of the collider

            if (hitObject.gameObject.tag == "Enemy") // checks if the target is an enemy and executes the code
            {
                //EnemyController enemy = hitObject.gameObject.transform.GetComponent<EnemyController>();
                //enemy.TakeDamage(damage * enemyDamageMultiplier);
            }
            if (hitObject.gameObject.tag == "Player") // checks if the target is an player and executes the code
            {
                gameManager.Player.GetComponent<playerController>().takeDamage(damage);
            }
            if (hitObject.gameObject.tag == "Enemy Building") // checks if the target is a building and executes the code
            {
                hitObject.gameObject.transform.GetComponent<StaticEnemy>().takeDamage(damage * buildingDamageMultiplier);
            }

            if (rigidBody != null) // checks if the target is a building and executes the code
            {
                rigidBody.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }
    }
}

