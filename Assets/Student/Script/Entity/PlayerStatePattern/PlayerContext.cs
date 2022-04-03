using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private GameObject lookAt;
    
    private IPlayerMoveState currState;
    private IPlayerMoveState oldState;

    [Header("Skills")]
    [SerializeField] private PlayerSkillMediator mediatorMB1;
    [SerializeField] private PlayerSkillMediator mediatorMB2;
    [SerializeField] private PlayerSkillMediator mediatorSPACE;
    private GameObject instanceOfSpaceSkill;

    [Header("Rigidbody & Colliders")]
    [SerializeField] private Rigidbody2D rb;

    [Header("BodyParts")]
    [SerializeField] private Transform spriteTr;
    [SerializeField] private GameObject hands;
    [SerializeField] private GameObject instantiateAt;

    [Header("Inputs")]
    [SerializeField] private float moveFactor = 2.0f;
    private float inputDirH;
    private float inputDirV;
    private bool inputCastMain = false;
    private bool inputCastSecondary = false;
    private bool inputCastSpace = false;
    private bool inputOption = false;

    [Header("Animator")]
    [SerializeField] private Animator anim;


    // SECTION - Property --------------------------------------------------------------------
    #region REGION - PROPERTY
    public PlayerSkillMediator MediatorMB1 { get => mediatorMB1; set => mediatorMB1 = value; }
    public PlayerSkillMediator MediatorMB2 { get => mediatorMB2; set => mediatorMB2 = value; }
    public PlayerSkillMediator MediatorSPACE { get => mediatorSPACE; set => mediatorSPACE = value; }
    public GameObject InstanceOfSpaceSkill { get => instanceOfSpaceSkill; set => instanceOfSpaceSkill = value; } // TransferLifeStrategy

    public Rigidbody2D Rb { get => rb; set => rb = value; }

    public GameObject Hands { get => hands; set => hands = value; }
    public GameObject InstantiateAt { get => instantiateAt; set => instantiateAt = value; }

    public float MoveFactor { get => moveFactor; set => moveFactor = value; }
    public float InputDirH { get => inputDirH; set => inputDirH = value; }
    public float InputDirV { get => inputDirV; set => inputDirV = value; }
    public bool InputCastMain { get => inputCastMain; set => inputCastMain = value; }
    public bool InputCastSecondary { get => inputCastSecondary; set => inputCastSecondary = value; }
    public bool InputCastSpace { get => inputCastSpace; set => inputCastSpace = value; }
    public bool InputOption { get => inputOption; set => inputOption = value; }

    public Animator Anim { get => anim; set => anim = value; }
    public GameObject LookAt { get => lookAt; set => lookAt = value; }
    #endregion


    // SECTION - Method - Unity -------------------------------------------------------------------
    private void Start()
    {
        currState = new StateGrounded();
        oldState = currState;
    }

    private void FixedUpdate()
    {
        if (oldState != currState)
        {
            oldState = currState;
            currState.OnStateEnter(this);
        }

        OnStateUpdate();
        OnStateExit();
    }

    // SECTION - Method - Context Specific -------------------------------------------------------------------
    public void FlipAll()
    {
        // Flip based on mouse position
        if (this.lookAt.transform.localPosition.x < 0 && spriteTr.localScale.x != -1.0f) // <- Flip All
            spriteTr.localScale = new Vector2(-1.0f, 1.0f);
        else if (this.lookAt.transform.localPosition.x > 0 && spriteTr.localScale.x != 1.0f) // -> Flip All
            spriteTr.localScale = new Vector2(1.0f, 1.0f);
    }


    // SECTION - Method - State Machine -------------------------------------------------------------------
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
}
