using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO
//      - Refactor Whole Hands rotation system;

public class StateGrounded : IPlayerMoveState
{
    // SECTION - Method - State Specific --------------------------------------------------------------------
    public void OnLook(PlayerContext context)
    {
        // Set look values of animation for animation transition based on cursor position
        context.Anim.SetFloat("mPosX", context.LookAt.transform.localPosition.x * 100);
        context.Anim.SetFloat("mPosY", context.LookAt.transform.localPosition.y * 100);

        // Set transform.up to point in direction of cursor
        // https://answers.unity.com/questions/585035/lookat-2d-equivalent-.html
        // HANDS      
        float x = context.Hands.transform.position.x - context.LookAt.transform.position.x;
        float y = context.Hands.transform.position.y - context.LookAt.transform.position.y;
        context.Hands.transform.up = new Vector3(x, y, context.Hands.transform.position.z);     
    }

    public void OnMove(PlayerContext context)
    {
        // Movement
        float moveH = context.InputDirH * context.MoveFactor;
        float moveV = context.InputDirV * context.MoveFactor;
        context.Rb.velocity = new Vector2(moveH, moveV);

        // Set Animator
        context.Anim.SetFloat("dirH", context.InputDirH);
        context.Anim.SetFloat("dirV", context.InputDirV);

        context.FlipAll();
    }

    public void OnCastMain(PlayerContext context)
    {
        if (context.InputCastMain && context.MediatorMB1.IsSkillReady())
        {
            context.MediatorMB1.InstantiateSkill(context);
            context.InputCastMain = false;
        }
    }

    public void OnCastSecondary(PlayerContext context)
    {
        if (context.InputCastSecondary && context.MediatorMB2.IsSkillReady())
        {
            context.MediatorMB2.InstantiateSkill(context);
            context.InputCastSecondary = false;
        }
    }

    public void OnCastSpace(PlayerContext context)
    {
        if (context.InputCastSpace && context.MediatorSPACE.IsSkillReady())
        {          
            context.MediatorSPACE.InstantiateSkill(context);
            
            context.InputCastSpace = false;
        }
    }

    // SECTION - Method - General --------------------------------------------------------------------
    public void OnStateEnter(PlayerContext context)
    {
    }

    public void OnStateUpdate(PlayerContext context)
    {
        OnLook(context);
        OnMove(context);
        OnCastMain(context);
        OnCastSecondary(context);
        OnCastSpace(context);
    }

    public IPlayerMoveState OnStateExit(PlayerContext context)
    {
        return this;
    }
}
