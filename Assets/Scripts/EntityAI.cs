using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    private EntityStats entityStats;
    private Rigidbody2D rb;
    private EntityStats attackTarget;
    private bool fogged = false;
    

    private float attackTimer;
    private Vector3 movementDir;

    // Start is called before the first frame update
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 1 /entityStats.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        fogged = entityStats.environmentEffect == EnvironmentEffects.Fog;
        
        foreach (EntityStats entity in GameManager.instance.allEntities)
        {
            //intially set attackTarget as anything other than itself
            if (entity != entityStats && entity.alliegiance != entityStats.alliegiance && entity.health > 0)
            {
                if (attackTarget == null)
                {
                    attackTarget = entity;
                    return;
                }
                else if (Vector3.Distance(gameObject.transform.position, entity.gameObject.transform.position)
                    < Vector3.Distance(gameObject.transform.position, attackTarget.transform.position))
                {
                    attackTarget = entity;
                }
            }
        }

        attackTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (attackTarget == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if(!fogged)
        {
            movementDir = attackTarget.transform.position - gameObject.transform.position;
        }
        rb.velocity = entityStats.speed * Time.fixedDeltaTime * movementDir.normalized;
        transform.up = movementDir;

        if (Vector3.Distance(gameObject.transform.position, attackTarget.transform.position) < entityStats.attackRange)
        {
            rb.velocity = Vector2.zero;
            TryAttack();
        }
    }

    void TryAttack()
    {
        if(attackTimer <= 0)
        {
            entityStats.animator.SetBool("doAttack", true);
            Attack();
        }
        else
            entityStats.animator.SetBool("doAttack", false);
    }

    void Attack()
    {
        attackTarget.TakeDamage(entityStats.attackDamage);
        attackTimer = 1 / entityStats.attackSpeed;
    }
}
