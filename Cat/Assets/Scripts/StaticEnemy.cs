using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    public float health = 100f;
    public float timeOfCollapse;
    float timer = 0f;
    public bool isDead = false;

    public void Update()
    {
        if(health <= 0 && !isDead)
        {
            Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>();
            gameObjectsRigidBody.mass = 2;
            Destroy(gameObject, timeOfCollapse);
            Destroy(gameObject.GetComponent<RocketTowerScript>());
            timer += Time.deltaTime * (1 / timeOfCollapse) / 10;
            gameObject.layer = LayerMask.GetMask("Ground");
            isDead = true;
        }
    }
    public void takeDamage(float damage)
    {
        health -= damage;
    }
}
