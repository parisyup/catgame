using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask enemy;

    public bool isPaused = false;

    public timeManager timeManager;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public int maxCollision;
    public float maxLifeTime;
    public float LifeTime;
    public bool explodeOnTouch = true;
    public bool explosive = false;

    int collisions;
    PhysicMaterial physicMaterial;


    private void Start()
    {
        setUp();
        
    }

    public void setUp()
    {
        LifeTime = maxLifeTime;
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = bounciness;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        gameObject.GetComponent<SphereCollider>().material = physicMaterial;
        rb.useGravity = useGravity;
    }

    private void Explode()
    {
        if (explosion != null)
        {
            GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(boom, 2f);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemy);

        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].GetComponent<StaticEnemy>())enemies[i].GetComponent<StaticEnemy>().takeDamage(explosionDamage);

            if (enemies[i].GetComponent<Rigidbody>()) enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        Invoke("Delay", 0.01f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        collisions++;

        if(collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    void Update()
    {
        if (isPaused) return;
        LifeTime -= Time.deltaTime;
        if ((collisions > maxCollision || LifeTime <= 0) && explosive) Explode();
        else if(collisions > maxCollision || LifeTime <= 0) Destroy(gameObject, 0.04f);
    }


}
