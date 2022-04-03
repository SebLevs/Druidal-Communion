using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferLifeStrategy : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private bool isPlayerSingleton = false; // Single instance max for player
    [SerializeField] private string[] canTransferFromTag;
    private Animator myAnim;

    [SerializeField] private GameObject myRegenPrefab;
    private GameObject myRegenInstance;
    [SerializeField] private Animator myRegenAnim;

    private ILivingEntity myTargetLE;
    private ILivingEntity myParentLE;

    private const float dmg = 1.0f;

    [SerializeField] private float tickEveryNSec = 2.0f;
    private const int maxTick = 3;
    private int currTick = 0;

    [SerializeField] private AudioSource asMain;


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    private void Start()
    {
        OnSingleInstanceOfPlayer();

        // Get Fields
        myAnim = GetComponent<Animator>();
        myRegenInstance = Instantiate(myRegenPrefab, transform.parent);
        myRegenAnim = myRegenInstance.GetComponent<Animator>();

        if (myParentLE == null)
            myParentLE = transform.parent.GetComponent<ILivingEntity>();

        // Main behavior
        InvokeRepeating("Execute", 0.0f, tickEveryNSec);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (string tag in canTransferFromTag)
        {
            if (other.tag == tag)
                ManageHp(other);
        }
    }


    // SECTION - Method - Utility --------------------------------------------------------------------
    private void ManageHp(Collider2D otherCol)
    {


        if (myParentLE.GetHp(false) != myParentLE.GetHp(true))
        {
            // Set sub animation for visual cue here 
            myRegenAnim.SetTrigger("isRegenHP");

            myTargetLE = otherCol.GetComponent<ILivingEntity>();
            myTargetLE.OnReceivingDamage(dmg);
            myParentLE.SetHp(myParentLE.GetHp(false) + dmg);
        }
    }

    private void OnSingleInstanceOfPlayer()
    {
        if (isPlayerSingleton)
        {
            PlayerContext pContextTemp = GameObject.Find("Player").GetComponent<PlayerContext>();

            if (pContextTemp.InstanceOfSpaceSkill == null)
                pContextTemp.InstanceOfSpaceSkill = this.gameObject;
            else
                Destroy(gameObject);
        }
    }


    // SECTION - Method - Implementation Specific --------------------------------------------------------------------
    public void Execute()
    {
        myAnim.SetTrigger("isAnimating");
    }

    private void AEAddTick() // Animation Event
    {
        currTick++;
    }

    private void AEDestroyOnMaxTick() // Animation Event
    {
        if (currTick == maxTick)
        {
            myRegenInstance.GetComponent<DestroyInTime>().DelegAllowDestr = true; // Setup instance for destruction on next enter of default state
            Destroy(gameObject);
        }
    }

    private void AEPlayAudioEffect() // Animation Event
    {
        asMain.PlayOneShot(asMain.clip);
    }

    private void AEStopAudioEffect()
    {
        asMain.Stop();
    }
}
