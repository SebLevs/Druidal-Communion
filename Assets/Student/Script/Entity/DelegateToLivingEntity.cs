using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateToLivingEntity : MonoBehaviour, ILivingEntity, IManageDotStrat
{
    // SECTION - Field -------------------------------------------------------------------
    [SerializeField] private LivingEntity delegateToLivingEntity; // To be set manually in inspector


    // SECTION - Method - Implementation -------------------------------------------------------------------
    public void OnReceivingDamage(float damage)
    {
        delegateToLivingEntity.OnReceivingDamage(damage);
    }

    public bool IsDead()
    {
        return delegateToLivingEntity.IsDead();
    }

    public GameObject GetDotStrat()
    {
        return delegateToLivingEntity.GetDotStrat();
    }

    public void SetDotStrat(GameObject dot)
    {
        delegateToLivingEntity.SetDotStrat(dot);
    }

    public float GetHp(bool isMaxHp)
    {
        return delegateToLivingEntity.GetHp(isMaxHp);
    }

    public void SetHp(float newHp)
    {
        delegateToLivingEntity.SetHp(newHp);
    }
}
