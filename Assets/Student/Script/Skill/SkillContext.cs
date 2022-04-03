using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillContext : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private Animator anim;

    [Header("CoolDown parameters")]
    [SerializeField] private float sCoolDown;
    [SerializeField] private SkillCastType sCastType; // [SkillCastType.cs]
    [SerializeField] private bool isUncastOnHit = false;

    [Header("Damage & Strategies")]
    [SerializeField] private int dmg;
    [SerializeField] private string[] canDamageTags;
    [SerializeField] private string[] canInteractTags;
    [SerializeField] private GameObject[] debuffStrategy;
    [SerializeField] private bool hasStandAloneStrategy = false;
    [SerializeField] private GameObject[] standaloneStrategy;


    // SECTION - Property --------------------------------------------------------------------
    public float SCoolDown { get => sCoolDown; set => sCoolDown = value; }
    public SkillCastType SCastType { get => sCastType; }


    // SECTION - Method - General --------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Note : Current game object is the trigger
        // Damage
        OnDamageEntity(other);

        // Debuffs
        OnApplyModToEntity(other);

        //Stand Alone strategies
        OnStandAloneStrategy(other); // other.gameObject.GetComponent<Collider2D>()
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Note : Current game object is the trigger
        // Damage
        OnDamageEntity(other.gameObject.GetComponent<Collider2D>());

        // Debuffs
        OnApplyModToEntity(other.gameObject.GetComponent<Collider2D>());

        //Stand Alone strategies
        OnStandAloneStrategy(other.gameObject.GetComponent<Collider2D>());
    }
    

    private void OnDamageEntity(Collider2D other)
    {
        if(dmg > 0) 
            foreach (string tag in canDamageTags)
            {
                // Check for tag
                if (other.tag == tag) // other.CompareTag(tag) : Threw bunch errors when used for unknown resons
                {
                    other.transform.GetComponent<ILivingEntity>().OnReceivingDamage(dmg);

                    UncastOnHitBehavior();
                }
            }
    }

    private void OnApplyModToEntity(Collider2D other)
    {
        if (canInteractTags != null)
            foreach (string tag in canInteractTags)
            {
                // Check for tag
                if (other.tag == tag)
                {
                    foreach (GameObject debuff in debuffStrategy)
                    {
                        // Apply debuffs to root to avoid placing it into toggleable part of the body
                        Instantiate(debuff, other.transform);
                    }
                    UncastOnHitBehavior();
                }
            }
    }

    private void OnStandAloneStrategy(Collider2D other)
    {
        if (hasStandAloneStrategy)
        {
            hasStandAloneStrategy = false;
            foreach (GameObject onCastBehavior in standaloneStrategy)
            {
                Instantiate(onCastBehavior, transform.position, transform.rotation);
            }
            UncastOnHitBehavior();
        }
    }

    private void UncastOnHitBehavior()
    {
        if (isUncastOnHit)
        {
            isUncastOnHit = false;
            anim.SetBool("hasHit", true);
            sCoolDown = 0.0f; // [SkillMediator.cs] will automatically set itself to usable again
        }
    }

    private void AEDestroyThis() // Animation Event
    {
        Destroy(gameObject);
    }

    private void AEBecomeStaticOnEndAnim() // AE
    {
        Destroy(this);
    }
}
