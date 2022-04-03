using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockEngagedState : IWarlockState
{    // SECTION - field --------------------------------------------------------------------
    private const float timerUntilIddle = 6.0f;
    private const float colTriggerRadius = 2.5f;


    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnAtkEnemyTp(WarlockContext context)
    { 
        // Note : Current Casting can be finished even after death
        if (!context.LeRef.IsDead())
            if (context.TpAtPlayerSkill.IsSkillReady() && context.PTransform != null)
            {
                // Set animation for casting visual cue
                // Skill is used at end of casting animation
                if (!context.Anim.GetBool("isCasting"))
                {
                    context.Anim.SetBool("isCasting", true);
                    context.Anim.SetTrigger("JahIthBer");
                    context.LastPTransform = context.PTransform;
                }
            }
    }

    public void OnAtkPlayerDmg(WarlockContext context)
    {
        // Note : Current Casting can be finished even after death
        if (!context.LeRef.IsDead())
            if (context.DmgSkill.IsSkillReady() && context.PTransform != null)
            {
                if (!context.Anim.GetBool("isCasting"))
                {
                    context.Anim.SetBool("isCasting", true);
                    context.Anim.SetTrigger("VexOhmIstDol");
                    context.LastPTransform = context.PTransform;
                }
            }
    }


    // SECTION - Method - General --------------------------------------------------------------------
    public void OnStateEnter(WarlockContext context)
    {
        context.ResetAnimBool();
        context.CastingColTrigger.radius = colTriggerRadius;
    }

    public void OnStateUpdate(WarlockContext context)
    {
            OnAtkPlayerDmg(context);
            OnAtkEnemyTp(context);
    }

    public IWarlockState OnStateExit(WarlockContext context)
    {
        // On death
        if (context.Anim.GetBool("isDead"))
            return context.OnDeath();
 
        
        // Check for -> IddleState
        if (!context.CastingColTrigger.IsTouchingLayers(context.TargetMask))
        {
            IWarlockState state = null;
            state = context.CheckReturnIddleWithTimer(timerUntilIddle);

            if (state != null)
                return state;
        }       

        return this;
    }
}
