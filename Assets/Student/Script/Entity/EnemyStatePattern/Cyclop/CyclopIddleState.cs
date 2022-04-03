using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopIddleState : ICyclopState
{
    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnSearch(CyclopContext context)
    {
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

        // Atk Triggers triggered by target
        if (context.AtkTriggerFront.IsTouchingLayers(context.TargetMask) || context.AtkTriggerAround.IsTouchingLayers(context.TargetMask))
        {
            context.Anim.SetTrigger("isEngaged");
            context.Anim.SetBool("isSearching", false);
            return new CyclopEngagedState();
        }

        // LOS triggered by target       
        if (context.Los.IsTouchingLayers(context.TargetMask) && !context.Anim.GetBool("isSearching"))
        {
            context.Anim.SetBool("isSearching", true);
            return new CyclopSearchState();
        }

        return this;
    }
}
