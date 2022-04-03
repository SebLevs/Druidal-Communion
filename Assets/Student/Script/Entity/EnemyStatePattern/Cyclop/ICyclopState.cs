using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICyclopState
{
    // SECTION - Method - State Specific --------------------------------------------------------------------
    void OnSearch(CyclopContext context);
    void OnAttack(CyclopContext context);


    // SECTION - Method - General --------------------------------------------------------------------
    void OnStateEnter(CyclopContext context);
    void OnStateUpdate(CyclopContext context);
    ICyclopState OnStateExit(CyclopContext context);
}
