using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopDeadState : ICyclopState
{
    // SECTION - Field --------------------------------------------------------------------
    private const string layer = "Interactible";


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
        if (context.Anim.GetBool("isDead"))
            context.Anim.SetBool("isDead", false); // Prevents looping of any state -> death anim
    }

    public void OnStateUpdate(CyclopContext context)
    {
        //OnSearch(context);
        //OnAttack(context);
    }
    public ICyclopState OnStateExit(CyclopContext context)
    {
        if (context.EntityCollider.IsTouchingLayers(LayerMask.GetMask(layer)))
            context.DestroyOnInteractibleCol();

        return this;
    }
}
