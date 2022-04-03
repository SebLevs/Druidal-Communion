using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotStategy : MonoBehaviour, ISkillStrategy
{
    // SECTION - Method --------------------------------------------------------------------
                     private ILivingEntity myTargetLivingEntity;
                     private IManageDotStrat myTargetDotStratManager;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private int tickQty;
    [SerializeField] private float tickTimer;
    [SerializeField] private int dmg;


    // SECTION - Method --------------------------------------------------------------------
    private void Start()
    {
        Execute();
    }

    public void Execute()
    {
        StartCoroutine(DamageOverTimeRootParent());
    }

    private IEnumerator DamageOverTimeRootParent()
    {
        // Set target
        myTargetLivingEntity = transform.parent.GetComponent<ILivingEntity>();
        myTargetDotStratManager = transform.parent.GetComponent<IManageDotStrat>();

        // Note: Delegatation Resets Dot on reapply
        myTargetDotStratManager.SetDotStrat(gameObject);

        // Behavior
        for (int tickCount = 0; tickCount < tickQty; tickCount++)
        {
            ps.Play();
            myTargetLivingEntity.OnReceivingDamage(dmg);
            yield return new WaitForSeconds(tickTimer);
        }

        Destroy(gameObject);

        yield return null;
    }
}
