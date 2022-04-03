using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockDeadState : IWarlockState
{
    // SECTION - Field --------------------------------------------------------------------
    private const string layer = "Interactible";


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
    }

    public void OnStateUpdate(WarlockContext context)
    {
       //OnAtkEnemyTp(context);
       //OnAtkPlayerTp(context);
       //OnAtkPlayerDmg(context);
    }
    public IWarlockState OnStateExit(WarlockContext context)
    {
        // Reminder : [atkColTeleP] is shortest range
        //      - Prevents blocking access to altarfor player
        if (context.EntityCollider.IsTouchingLayers(LayerMask.GetMask(layer)))
            context.DestroyOnInteractibleCol();

        return this;
    }
}
