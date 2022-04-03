using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, ILivingEntity, IManageDotStrat
{
    // SECTION - Field -------------------------------------------------------------------
    [Header("General Stats")]
    [SerializeField] private float hpMax = 20.0f;
    [SerializeField] private float hp = 20.0f;
                     private GameObject dotStrat = null;

    [Header("Components")]
    [SerializeField] private SpriteRenderer sprRdr;
    [SerializeField] private Image hpbarImage;
    [SerializeField] private Text hpbarTxt;
                     private Animator anim;

    [Header("iframe")]
    private bool isIFrame = false;
    private const float cueNDeathTimer = 0.20f; // 0.20f

    [Header("Audio")]
    [SerializeField] private AudioSource audioS;


    // SECTION - Property -------------------------------------------------------------------
    public bool IsIFrame { get => isIFrame; set => isIFrame = value; }



    // SECTION - Method - General -------------------------------------------------------------------
    private void Start()
    {
        if (!gameObject.CompareTag("Enemy"))
            RefreshTextBar();


        if (!gameObject.CompareTag("Player"))
            anim = GetComponent<Animator>();
        else
            anim = GetComponentInChildren<Animator>();
    }

    // SECTION - Method - Implementation -------------------------------------------------------------------
    public void OnReceivingDamage(float damage) 
    {
        // Preventing stunlock through bool
        if (!isIFrame && !IsDead())
        {
            // iframes
            isIFrame = true;

            //  [Enemies] - Active on first damage received       
            if (!hpbarTxt.transform.parent.gameObject.activeSelf)
            {
                if (gameObject.CompareTag("Enemy"))
                    anim.SetTrigger("isEngaged");

                ToggleHpBar();
            }

            // HP manager
            ReceiveDamage(damage);
            audioS.PlayOneShot(audioS.clip);

            RefreshTextBar();
            hpbarImage.fillAmount = hp / hpMax;

            // Visual Cue
            sprRdr.color = Color.red;
            Invoke("ResetColor", cueNDeathTimer);
        }
    }

    public bool IsDead()
    {
        return this.hp <= 0;
    }

    public float GetHp(bool isMaxHp)
    {
        if (isMaxHp)
            return hpMax;

        return hp;
    }

    public void SetHp(float newHp)
    {
        if (newHp > hpMax)
            hp = hpMax;
        else if (newHp <= 0)
        {
            hp = 0;
            OnDeathManager();
        }
        else
            hp = newHp;

        RefreshTextBar();
        hpbarImage.fillAmount = hp / hpMax;
    }


    public GameObject GetDotStrat()
    {
        return this.dotStrat;
    }

    public void SetDotStrat(GameObject dot)
    {
        if (this.dotStrat != null)
            Destroy(this.dotStrat);

        this.dotStrat = dot;
    }


    // SECTION - Method - Utility -------------------------------------------------------------------
    private void ResetColor() // Used in [OnReceivingDamage]
    {
        sprRdr.color = Color.white;
        isIFrame = false;
        OnDeathManager();
    }

    public void RefreshTextBar()
    {
        hpbarTxt.text = $"{hp} / {hpMax}";
    }

    private void OnDeathManager()
    {
        // Note
        //      - Uppon death of enemy : OnDeath() is also called at death animaton | loc : [xxxContext.cs]
        if (IsDead())
        {
            if (!gameObject.CompareTag("Player"))
                anim.SetTrigger("isDead");
            else if (gameObject.CompareTag("Player"))
                GameManager.instance.GenericGameOver();
        }
    }

    private void ReceiveDamage(float damage)
    {
        if (this.hp - damage < 0)
            this.hp = 0;
        else
            this.hp -= damage;
    }

    public void ToggleHpBar()
    {
        bool temp = hpbarTxt.transform.parent.gameObject.activeSelf ? false : true;
        hpbarTxt.transform.parent.gameObject.SetActive(temp);

        if (IsDead())
            DeactivateHpBar();

    }    
    
    public void DeactivateHpBar()
    {
        hpbarTxt.transform.parent.gameObject.SetActive(false);
    }

    private void AEDestroyThis() // Animator Event
    {
        Destroy(gameObject);
    }

    // Prevents disapearance of certain skills OnCollisionEnter with dead body of enemy
    private void AEOnDeath() // Animator Event
    {
        ToggleHpBar();
        tag = "Untagged";
    }
}
