using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Alliegiance alliegiance;
    public enum AttackType { melee, ranged}
    public AttackType attackType;
    
    [Header("Stats")]
    public float health;
    public float speed;
    public float attackSpeed;
    public float attackDamage;
    public float attackRange;

    [HideInInspector] public float defenseMult = 1;

    [Header("Affected By")]
    public MindEffects mindEffect;
    public BodyEffects bodyEffect;
    public EnvironmentEffects environmentEffect;
    [Space]
    public SpriteRenderer mindIndicator;
    public SpriteRenderer bodyIndicator;
    public SpriteRenderer environmentIndicator;

    private float mindEffectTimer;
    private float bodyEffectTimer;
    private float environmentEffectTimer;

    private Alliegiance originalAlliegiance;
    private float baseSpeed;
    private float baseAttackSpeed;
    private float baseAttackDamage;
    private float baseAttackRange;

    private bool hasMedicine = false;



    // Start is called before the first frame update
    void Start()
    {
        originalAlliegiance = alliegiance;
        baseSpeed = speed;
        baseAttackSpeed = attackSpeed;
        baseAttackDamage = attackDamage;
        baseAttackRange = attackRange;

        mindIndicator.enabled = false;
        bodyIndicator.enabled = false;
        environmentIndicator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Death();

        if (mindEffectTimer <= 0)
        {
            mindEffect = 0;
        }
        mindEffectTimer -= Time.deltaTime;

        if (bodyEffectTimer <= 0)
        {
            bodyEffect = 0;
        }
        bodyEffectTimer -= Time.deltaTime;

        if(hasMedicine)
        {
            health += 5 * Time.deltaTime;
        }

        HandleMind();
        HandleBody();
        HandleEnvironment();
        

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    private void Death()
    {
        GameManager.instance.pickupSpawnCount--;
        Destroy(gameObject);
    }

    void HandleMind()
    {
        //reset effects
        alliegiance = originalAlliegiance;
        attackSpeed = baseAttackSpeed;
        attackDamage = baseAttackDamage;
        defenseMult = 1;
        speed = baseSpeed;

        switch (mindEffect)
        {
            case MindEffects.Charm:
                //actual effects
                if (originalAlliegiance == Alliegiance.Good_Guys) alliegiance = Alliegiance.Bad_Guys;
                else if (originalAlliegiance == Alliegiance.Bad_Guys) alliegiance = Alliegiance.Good_Guys;
                mindIndicator.color = new Color32(0xCF,0x4E,0xC3,0xFF);
                mindIndicator.enabled = true;

                break;
            case MindEffects.Frenzy:
                //actual effects
                alliegiance = Alliegiance.none;
                attackSpeed = baseAttackSpeed * 1.2f;
                speed = baseSpeed * 1.5f;
                mindIndicator.color = new Color32(0xEC,0xE0,0x1E,0xFF);
                mindIndicator.enabled = true;

                break;
            case MindEffects.MoraleBoost:
                //actual effects
                attackSpeed = baseAttackSpeed * 1.2f;
                attackDamage = baseAttackDamage * 1.2f;
                defenseMult = 1.2f;
                mindIndicator.color = new Color32(0x14,0xC3,0xFB, 0xFF);
                mindIndicator.enabled = true;

                break;
            default:
                mindIndicator.enabled = false;
                break;
        }

    }
    void HandleBody()
    {
        //reset effects
        defenseMult = 1;
        attackDamage = baseAttackDamage;
        hasMedicine = false;

        switch (bodyEffect)
        {
            case BodyEffects.Fraility:
                //actual effects
                defenseMult = 0.7f;
                attackDamage = baseAttackDamage * 0.7f;
                bodyIndicator.color = new Color32(0xC5,0x22,0xDA,0xFF);
                bodyIndicator.enabled = true;

                break;
            case BodyEffects.Medicine:
                //actual effects
                hasMedicine = true;
                bodyIndicator.color = new Color32(0xF3,0x31,0x31,0xFF);
                bodyIndicator.enabled = true;
                break;
            default:
                bodyIndicator.enabled = false;
                break;
        }
    }
    void HandleEnvironment()
    {
        if (environmentEffectTimer <= 0)
        {
            environmentEffect = 0;
            return;
        }
        environmentEffectTimer -= Time.deltaTime;
        //reset effect for when not in environment
        attackRange = baseAttackRange;
        speed = baseSpeed;
        switch(environmentEffect)
        {
            case EnvironmentEffects.DOT:
                TakeDamage(5 * Time.deltaTime);
                environmentIndicator.color = new Color32(0xFF,0xA6,0x00,0xFF);
                environmentIndicator.enabled = true;
                break;
            case EnvironmentEffects.Fog:
                attackRange = Mathf.Min(baseAttackRange, 2.5f);
                environmentIndicator.color = new Color32(0xB2,0xB2,0xB2,0xFF);
                environmentIndicator.enabled = true;
                break;

            case EnvironmentEffects.Mud:
                speed = baseSpeed * 0.7f;
                environmentIndicator.color = new Color32(0x9D, 0x62, 0x11, 0xFF);
                environmentIndicator.enabled = true;
                break;
            default:
                environmentIndicator.enabled = false;
                break;
        }
    }

    public void ApplyEffect(Effects effect, float duration)
    {
        if(effect.mindEffect > 0)
        {
            mindEffect = effect.mindEffect;
            mindEffectTimer = duration;
        }
        if (effect.bodyEffect > 0)
        {
            bodyEffect = effect.bodyEffect;
            bodyEffectTimer= duration;
        }
        //maybe have multiple environment effects at same time?
        if (effect.environmentEffect > 0)
        {
            environmentEffect = effect.environmentEffect;
            environmentEffectTimer = duration;
            //HandleEnvironment() is in Update()
        }
    }
    
}


