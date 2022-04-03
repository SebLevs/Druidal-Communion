using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectSpawner : MonoBehaviour, IObjectSpawner
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private bool isExecuteAtStart = false;

    [Header("Numeric values")]
    [SerializeField] private int maxSpawnable = 10;
    [Range(0.0f, 0.64f)] [SerializeField] private float additonalSpawnRange;

    [Header("Spawnable Section")]
    [SerializeField] private LayerMask recursiveSpawnOnHit;
    [Tooltip("Leave empty if you want to spawn as child of [if No Parent Find This] variable")]
    [SerializeField] private Transform desiredParentTransform;
    [SerializeField] private string ifNoDesiredParentFindThis = "MiscDumpster";
    [SerializeField] private GameObject[] mySpawnablePrefabs;
    [SerializeField] private List<GameObject> mySpawnableList = new List<GameObject>();


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    private void Start()
    {
        if (isExecuteAtStart)
            Execute();
    }


    // SECTION - Method - Implementation --------------------------------------------------------------------
    public void Execute()
    {
        for (int i = 0; i < maxSpawnable; i++)
        {
            Vector2 spawnAt = RecursiveSpawnCheck(transform);

            int randEnemyPrefab = UnityEngine.Random.Range(0, mySpawnablePrefabs.Length);
            mySpawnableList.Add(Instantiate(mySpawnablePrefabs[randEnemyPrefab], spawnAt, Quaternion.identity));

            if (desiredParentTransform != null)
                mySpawnableList[i].transform.parent = desiredParentTransform;
            else
                mySpawnableList[i].transform.parent = GameObject.FindWithTag(ifNoDesiredParentFindThis).transform;
        }
    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }

    public List<GameObject> GetMySpawnableList()
    {
        return this.mySpawnableList;
    }


    // SECTION - Method - Utility --------------------------------------------------------------------
    private Vector2 RecursiveSpawnCheck(Transform spawnNear)
    {
        int minusOrPositiveX = (UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1;
        int minusOrPositiveY = (UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1;

        float spawnAtX = UnityEngine.Random.Range(transform.position.x + additonalSpawnRange, transform.position.x + additonalSpawnRange * 2 * minusOrPositiveX);
        float spawnAtY = UnityEngine.Random.Range(transform.position.y + additonalSpawnRange, transform.position.y + additonalSpawnRange * 2 * minusOrPositiveY);

        Vector2 temp = new Vector2(spawnAtX, spawnAtY);

        // Recursively get new position based on this.object collision with specified layermask
        Collider2D collidedWith = Physics2D.OverlapCircle(temp, GameManager.instance.avrgTileSize, recursiveSpawnOnHit);
        if (collidedWith != null)
            temp = RecursiveSpawnCheck(spawnNear);

        return temp;
    }
}
