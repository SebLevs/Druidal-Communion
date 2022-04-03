using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockContext : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private IWarlockState currState;
    [SerializeField] private IWarlockState oldState;

    [Header("Skills")]
    [SerializeField] private EnemySkillMediator dmgSkill;
    [SerializeField] private EnemySkillMediator tpAtPlayerSkill;
    private LivingEntity leRef;

    [Header("Rigidbody & Colliders")]
    [SerializeField] private Collider2D entityCollider;
    [SerializeField] private CircleCollider2D castingColTrigger;

    [Header("Sound")]
    [SerializeField] private AudioSource asCasting;

    [Header("Misc fields")]
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private GameObject[] childrenCol;
    private Animator anim;
    private Transform pTransform;
    private Transform lastPTransform = null;
    private float currtimerNoPlayerLOS = 0.0f;


    // SECTION - Property --------------------------------------------------------------------
    #region PROPERTY
    public EnemySkillMediator DmgSkill { get => dmgSkill; set => dmgSkill = value; }
    public EnemySkillMediator TpAtPlayerSkill { get => tpAtPlayerSkill; set => tpAtPlayerSkill = value; }
    public LivingEntity LeRef { get => leRef; set => leRef = value; }

    public CircleCollider2D CastingColTrigger { get => castingColTrigger; set => castingColTrigger = value; } // Long range : Also LOS
    public Collider2D EntityCollider { get => entityCollider; set => entityCollider = value; }

    public Animator Anim { get => anim; set => anim = value; }

    public LayerMask TargetMask { get => targetMask; }
    public Transform PTransform { get => pTransform; set => pTransform = value; }
    public Transform LastPTransform { get => lastPTransform; set => lastPTransform = value; }


    #endregion


    // SECTION - Method - Unity -------------------------------------------------------------------
    private void Start()
    {
        // Set field
        anim = GetComponent<Animator>();
        leRef = GetComponent<LivingEntity>();

        // Set state
        currState = new WarlockIddleState();
    }

    private void Update()
    {
        if (oldState != currState)
        {
            oldState = currState;
            currState.OnStateEnter(this);
        }

        OnStateUpdate();
        OnStateExit();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            pTransform = other.transform.root.GetComponent<Transform>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ResetLosTimer();
            lastPTransform = PTransform;
            pTransform = null;
        }
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

    public IWarlockState OnDeath()
    {
        anim.SetBool("isDead", false);
        return new WarlockDeadState();
    }

    public void OnTriggerDetect()
    {
        // Reminder : [atkcol] is longest range of all colliders
        if (castingColTrigger.IsTouchingLayers(targetMask))
            currtimerNoPlayerLOS = 0.0f;
    }

    public void ResetLosTimer()
    {
        currtimerNoPlayerLOS = 0.0f;
    }
    public void AESetChildrenInactive() // Animator Event
    {
        foreach (GameObject obj in this.childrenCol)
            obj.SetActive(false);
    }

    public IWarlockState CheckReturnIddleWithTimer(float timerUntilerIddle)
    {
        IWarlockState state = null;

        currtimerNoPlayerLOS += Time.deltaTime;

        if (currtimerNoPlayerLOS >= timerUntilerIddle)
        {
            anim.SetTrigger("isIddle");
            state = new WarlockIddleState();
            ResetLosTimer();
        }

        return state;
    }

    public void DestroyOnInteractibleCol()
    {
        Destroy(gameObject);
    }

    public void AEOnDamageIddle() // Animation Event
    {
        currState = new WarlockEngagedState();
    }

    public void CastVexOhmIstDol() // Animation Event
    {
        if (lastPTransform != null)
            dmgSkill.InstantiateSkill(lastPTransform);
    }

    public void CastJahIthBer() // Animation Event
    {
        if (lastPTransform != null)
            tpAtPlayerSkill.InstantiateSkill(lastPTransform);
    }

    public void AEOnAllowCasting() // Animation Event
    {
        if (anim.GetBool("isCasting"))
            anim.SetBool("isCasting", false);
    }

    public void ResetAnimBool()
    {
        anim.SetBool("isEngaged", false);
    }

    private void AEPlayCastingAudio() // Animation Event
    {
        float pitchRand = Random.Range(0.5f, 0.75f);
        asCasting.PlayOneShot(asCasting.clip);
        asCasting.pitch = pitchRand;
    }

    private void AEStopCastingAudio()
    {
        asCasting.Stop();
    }

}
