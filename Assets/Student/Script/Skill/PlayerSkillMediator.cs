using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class PlayerSkillMediator : MonoBehaviour
{
    /// <summary>
    /// 
    /// Note:
    ///     - Skills are called inside of player Movement State Machine
    ///         + Player's state machine delegate to InstantiateSkill()
    ///         + skillInstance take care of its own behavior
    ///             ++ SkillCooldown is taken care 
    ///             
    ///     - sContext should be set either:
    ///         + Before Game Start : Manually
    ///         + In-Game           : Delegate through SetPrefabAndContext() 
    /// 
    /// </summary>

    // SECTION - Field --------------------------------------------------------------------
    [Header("Skill Parameters")]
    [SerializeField] private GameObject sPrefab;
    [SerializeField] private SkillContext sContext;
                     private GameObject sInstance;


    [Header("CoolDown Parameters")]
    [SerializeField] private Image cdMaskImage;
    [SerializeField] private GameObject cdTimerbckg;
    [SerializeField] private Text cdTimerTxt;
                     private float currCdTimer = 0.0f;


    // SECTION - Method --------------------------------------------------------------------
    public void InstantiateSkill(PlayerContext context)
    {
        switch (sContext.SCastType)
        {
            case SkillCastType.MELEE:
                    InstantiateTowardTarget(context);
                    break;
            case SkillCastType.ATCURRENTPOS:
                    InstantiateAtPlayerTr(context);
                    break;
            case SkillCastType.ATCURSOR:
                    InstantiateAtCursor(context);
                    break;
            default: Debug.Log("An error has occured at [InstantiateSkill()] - [PlayerSkillMediator.cs]"); break;
        }
    }

    private void InstantiateTowardTarget(PlayerContext context)
    {
        // https://gamedev.stackexchange.com/questions/181653/follow-behaviour-with-transform-lookat-on-unity2d
        Vector2 direction = (context.LookAt.transform.position - context.InstantiateAt.transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        context.InstantiateAt.transform.rotation = Quaternion.Euler(Vector3.forward * angle); // var offset = 90f; && (angle+90)

        InstantiateAndSetSkill(context.InstantiateAt.transform.position + context.InstantiateAt.transform.transform.right * GameManager.instance.avrgTileSize, context.InstantiateAt.transform.rotation);

        StartCoroutine(StartCooldown());
    }

    private void InstantiateAtPlayerTr(PlayerContext context)
    {
        InstantiateAndSetSkill(context.transform.position, context.transform.rotation);
        StartCoroutine(StartCooldown());
    }


    private void InstantiateAtCursor(PlayerContext context)
    {
        // Get spawn pos [local pos Mouse Cursor]
        Vector2 spawnAt = new Vector2(context.LookAt.transform.position.x, context.LookAt.transform.position.y);

        InstantiateAndSetSkill(spawnAt, Quaternion.identity);
        StartCoroutine(StartCooldown());
    }

    private void InstantiateAndSetSkill(Vector2 tr, Quaternion rotation)
    {
        sInstance = Instantiate(sPrefab, tr, rotation);
        sContext = sInstance.GetComponent<SkillContext>();
    }

    private void InstantiateAndSetSkill(Transform transform)
    {
        sInstance = Instantiate(sPrefab, transform);
        sContext = sInstance.GetComponent<SkillContext>();
    }

    public IEnumerator StartCooldown()
    {
        cdTimerbckg.SetActive(true);

        do
        {
            currCdTimer += Time.deltaTime;
            cdTimerTxt.text = Math.Round((sContext.SCoolDown - currCdTimer), 1).ToString(); // Set TEXT
            cdMaskImage.fillAmount = currCdTimer / sContext.SCoolDown; // set FILLER MASK

            yield return new WaitForSeconds(Time.deltaTime);
        } while (currCdTimer < sContext.SCoolDown);

        OnCooldownEnd();

        yield return null;
    }

    private void OnCooldownEnd()
    {
        currCdTimer = 0;
        cdMaskImage.fillAmount = 0.0f;
        cdTimerbckg.SetActive(false);
    }

    public bool IsSkillReady()
    {
        return currCdTimer == 0;
    }
}

