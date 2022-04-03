using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopEngagedState : ICyclopState
{
    // SECTION - field --------------------------------------------------------------------
    private const float timerUntilIddle = 6.0f;


    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnSearch(CyclopContext context)
    {
        // Reset timer -> CyclopIddleState on Los collision
        context.OnTriggerDetect();
    }

    public void OnAttack(CyclopContext context)
    {
        // Note
        //      - GameObject with colliders are the one dealing damage
        //      - GameObject with colliders are SetActive() through Animator event system
        if (context.AtkTriggerFront.IsTouchingLayers(context.TargetMask)) // Front
            context.Anim.SetTrigger("pIsFront");
        else if (context.AtkTriggerAround.IsTouchingLayers(context.TargetMask)) // Around
            context.Anim.SetTrigger("pIsAround");      
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

        // Check for -> IddleState
        if (!context.Los.IsTouchingLayers(context.TargetMask))
        {
            ICyclopState state = null;
            state = context.CheckReturnIddleWithTimer(timerUntilIddle);

            if (state != null)
                return state;
        }

        return this;
    }
}
