using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectSpawner
{
    // SECTION - Method --------------------------------------------------------------------
    void Execute();
    GameObject GetThisObject();
    List<GameObject> GetMySpawnableList();
}
