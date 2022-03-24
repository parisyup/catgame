using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCode : MonoBehaviour
{
    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;
    public SpiderGun gun1;
    public SpiderGun gun2;
    public EnemyTurretController targetting;
    public SamTurret sam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameManager.instance.Player.transform;
    }

    void Update()
    {
        if (GameManager.instance.timeManager.GetComponent<timeManager>().isPaused)
        {
            agent.SetDestination(transform.position);
            return;
        }
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > lookRadius) return;
        agent.SetDestination(target.position);

        if (distance < agent.stoppingDistance)
        {
            FaceTarget();
            if (sam != null) { if (sam.targets.Length > 0) { Attack(); } else StopAttacking(); }
            else { Attack();  }
            

            
            
        }
        else StopAttacking();

        
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void Attack()
    {
        gun1.isAttacking(false);
        gun2.isAttacking(false);
        targetting.isAttacking(false);
    }    
    public void StopAttacking()
    {
        gun1.isAttacking(true);
        gun2.isAttacking(true);
        targetting.isAttacking(true);
    }
}
