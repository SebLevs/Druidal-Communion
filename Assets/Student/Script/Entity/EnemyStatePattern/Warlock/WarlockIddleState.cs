using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockIddleState : IWarlockState
{
    // SECTION - Field --------------------------------------------------------------------
    private const float colTriggerRadius = 1.0f;


    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnAtkEnemyTp(WarlockContext context)
    {

    }

    public void OnAtkPlayerDmg(WarlockContext context)
    {

    }


    // SECTION - Method - General --------------------------------------------------------------------
    public void OnStateEnter(WarlockContext context)
    {
        context.ResetAnimBool();
        context.CastingColTrigger.radius = colTriggerRadius;
    }

    public void OnStateUpdate(WarlockContext context)
    {
        //OnAtkEnemyTp(context);
        //OnAtkPlayerDmg(context);
    }

    public IWarlockState OnStateExit(WarlockContext context)
    {
        // On death
        if (context.Anim.GetBool("isDead"))
            return context.OnDeath();

        // Reminder : AtkCol is longest range : If it touches, it can act
        if (context.CastingColTrigger.IsTouchingLayers(context.TargetMask))
        {
            context.Anim.SetTrigger("isEngaged");
            return new WarlockEngagedState();
        }

        return this;
    }
}
