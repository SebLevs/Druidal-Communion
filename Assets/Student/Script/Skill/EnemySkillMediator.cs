using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillMediator : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [Header("Skill Parameters")]
    [SerializeField] private GameObject sPrefab;
    [SerializeField] private SkillContext sContext;

    public float currCdTimer = 0.0f;


    // SECTION - Method --------------------------------------------------------------------
    public void InstantiateSkill(Transform targetTransform) // Todo refactor if time : Switch antipattern
    {
        switch (sContext.SCastType)
        {
            case SkillCastType.ATSPECIFICTARGET:
                InstantiateAtTarget(targetTransform);
                break;
            default: Debug.Log("An error has occured at [InstantiateSkill()] - [EnemySkillMediator.cs]"); break;
        }
    }

    private void InstantiateAtTarget(Transform targetTransform)
    {
        StartCoroutine(StartCooldown());
        Instantiate(sPrefab, targetTransform.position, targetTransform.rotation);
    }

    public IEnumerator StartCooldown()
    {
        currCdTimer += Time.deltaTime;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            currCdTimer += Time.deltaTime;
        } while (currCdTimer < sContext.SCoolDown);

        OnCooldownEnd();

        yield return null;
    }

    private void OnCooldownEnd()
    {
        currCdTimer = 0;
    }

    public bool IsSkillReady()
    {
        return currCdTimer == 0;
    }
}
