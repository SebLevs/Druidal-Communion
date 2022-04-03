using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWarlockState
{
    // SECTION - Method - State Specific --------------------------------------------------------------------
    void OnAtkEnemyTp(WarlockContext context);
    void OnAtkPlayerDmg(WarlockContext context);


    // SECTION - Method - General --------------------------------------------------------------------
    void OnStateEnter(WarlockContext context);
    void OnStateUpdate(WarlockContext context);
    IWarlockState OnStateExit(WarlockContext context);
}
