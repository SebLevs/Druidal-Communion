using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopContext : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private ICyclopState currState;
    [SerializeField] private ICyclopState oldState;

    [Header("Rigidbody & Colliders")]
    [SerializeField] private Collider2D los; // Target detection -> isSearching [Animator]
    [SerializeField] private Collider2D atkTriggerFront; // Whenever target is close enough
    [SerializeField] private Collider2D atkTriggerAround; // Whenever target is close enough
    [SerializeField] private Collider2D entityCollider;
    [SerializeField] private GameObject[] childrenCol = new GameObject[5];

    [Header("Misc fields")]
    [SerializeField] private LayerMask targetMask;
                     private Animator anim;
                     private LivingEntity livingEntity;
                     private float timerNoPlayerLOS = 0.0f;


    // SECTION - Property --------------------------------------------------------------------
    #region PROPERTY
    public Collider2D Los { get => los; set => los = value; }
    public Collider2D AtkTriggerFront { get => atkTriggerFront; set => atkTriggerFront = value; }
    public Collider2D AtkTriggerAround { get => atkTriggerAround; set => atkTriggerAround = value; }
    public Collider2D EntityCollider { get => entityCollider; set => entityCollider = value; }
    public LayerMask TargetMask { get => targetMask; set => targetMask = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public LivingEntity LivingEntity { get => livingEntity; set => livingEntity = value; }
    //public float TimerNoPlayerLOS { get => timerNoPlayerLOS; set => timerNoPlayerLOS = value; }

    #endregion


    // SECTION - Method - Unity -------------------------------------------------------------------
    private void Start()
    {
        // Set field
        anim = GetComponent<Animator>();
        livingEntity = GetComponent<LivingEntity>();

        // Set state
        currState = new CyclopIddleState();
    }

    private void Update()
    {
        if (oldState != currState)
        {
            if (timerNoPlayerLOS > 0.0f)
                ResetLosTimer();

            oldState = currState;
            currState.OnStateEnter(this);
        }

        OnStateUpdate();
        OnStateExit();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            ResetLosTimer();
    }


    // SECTION - Method - Context Specific -------------------------------------------------------------------
    public void OnStateEnter()
    {
        currState.OnStateEnter(this);
    }

    public void OnStateUpdate()
    {
        currState.OnStateUpdate(this);
    }

    public void OnStateExit()
    {
        currState = currState.OnStateExit(this);
    }

    public void AESetChildrenInactive() // Animator Event
    {
        foreach (GameObject obj in this.childrenCol)
            obj.SetActive(false);

        if (!livingEntity.IsDead())
            entityCollider.gameObject.SetActive(true);

    }

    public ICyclopState OnDeath()
    {
        anim.SetBool("isDead", false);
        return new CyclopDeadState();
    }

    public void OnTriggerDetect()
    {
        if (los.IsTouchingLayers(targetMask) || atkTriggerFront.IsTouchingLayers(targetMask) || atkTriggerAround.IsTouchingLayers(targetMask))
            timerNoPlayerLOS = 0.0f;
    }

    public void ResetLosTimer()
    {
        timerNoPlayerLOS = 0.0f;
    }

    public ICyclopState CheckReturnIddleWithTimer(float timerUntilerIddle)
    {
        ICyclopState state = null;

        timerNoPlayerLOS += Time.deltaTime;

        if (timerNoPlayerLOS >= timerUntilerIddle)
        {
            anim.SetTrigger("isIddle");
            state = new CyclopIddleState();
        }

        return state;
    }

    public void DestroyOnInteractibleCol()
    {
        Destroy(gameObject);
    }

    public void AETowardsEngagedState() // Animation Event 
    {
        // Set state machine to engaged when engaged animation triggered outside this.state machine
        if (!(currState is CyclopEngagedState))
            currState = new CyclopEngagedState();
    }

    public void ResetAnimBool()
    {
        anim.SetBool("isEngaged", false);
        anim.SetBool("isSearching", false);
    }
}
