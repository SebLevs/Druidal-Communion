using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopSearchState : ICyclopState
{
    // SECTION - field --------------------------------------------------------------------
    private const float timerUntilIddle = 3.0f;


    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnSearch(CyclopContext context)
    {
        // Reset timer -> CyclopIddleState on Los collision
        context.OnTriggerDetect();
    }

    public void OnAttack(CyclopContext context)
    {
    }


    // SECTION - Method - General --------------------------------------------------------------------
    public void OnStateEnter(CyclopContext context)
    {
        context.ResetAnimBool();
    }

    public void OnStateUpdate(CyclopContext context)
    {
        OnSearch(context);
        OnAttack(context);
    }

    public ICyclopState OnStateExit(CyclopContext context)
    {
        // On death
        if (context.Anim.GetBool("isDead"))
            return context.OnDeath();

        // On being Attacked
        if (context.LivingEntity.IsIFrame)
            return new CyclopEngagedState();

        // Check for -> CyclopIddleState
        if (!context.Los.IsTouchingLayers(context.TargetMask))
        {
            ICyclopState state = null;
            state = context.CheckReturnIddleWithTimer(timerUntilIddle);

            if (state != null)
            {
                context.Anim.SetBool("isSearching", false);
                return state;
            }
        }

        // Atk Triggers triggered by target
        if (context.AtkTriggerFront.IsTouchingLayers(context.TargetMask) || context.AtkTriggerAround.IsTouchingLayers(context.TargetMask))
        {
            context.Anim.SetTrigger("isEngaged");
            context.Anim.SetBool("isSearching", false);
            return new CyclopEngagedState();
        }

        return this;
    }
}
