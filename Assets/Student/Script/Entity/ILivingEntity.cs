using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILivingEntity
{
    // SECTION - Method -------------------------------------------------------------------
    void OnReceivingDamage(float damage);

    bool IsDead();

    float GetHp(bool isMaxHp);

    void SetHp(float newHp);
}
