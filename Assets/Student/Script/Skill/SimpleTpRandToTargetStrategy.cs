using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTpRandToTargetStrategy : MonoBehaviour, ISkillStrategy
{
    // SECTION - Method --------------------------------------------------------------------
    [Header("Tags & Masks")]
    [SerializeField] string[] teleportableTags;
    [SerializeField] private LayerMask avoidTpAtMask;

    [Header("Available Tp location")]
    [SerializeField] private Collider2D[] canTpAtColls;
                     private List<Transform> teleportableList = new List<Transform>();


    // SECTION - Method --------------------------------------------------------------------
    private void Start()
    {
        StartCoroutine(DelayedCast()); // 0.25f second delay to allow OnTriggerEnter2D() to do its job
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        ILivingEntity oLivingEntity = other.transform.GetComponent<ILivingEntity>();
        if (oLivingEntity != null && !oLivingEntity.IsDead())
        {
            // Prevents selecting body members
            Transform eTransform = other.transform.GetComponent<Transform>();

            foreach (string tag in teleportableTags)
            {
                if (other.gameObject.CompareTag(tag) && !teleportableList.Contains(eTransform))
                    teleportableList.Add(eTransform);
            }
        }    
    }
    // SECTION - Method --------------------------------------------------------------------
    public void Execute()
    {
        Collider2D tpAtCol = GetValidTpPos();

        if (tpAtCol != null && teleportableList.Count > 0)
        {
            int randIndex = Random.Range(0, teleportableList.Count);
            teleportableList[randIndex].position = tpAtCol.transform.position;
            tpAtCol.GetComponent<Animator>().enabled = true;
        }            
    }

    // SECTION - Utility  --------------------------------------------------------------------
    private Collider2D GetValidTpPos()
    {
        Collider2D returnthis = null;
        foreach (Collider2D tpCol in canTpAtColls)
        {
            if (!tpCol.IsTouchingLayers(avoidTpAtMask))
            {
                returnthis = tpCol;
                break;
            }          
        }

        return returnthis;
    }

    private IEnumerator DelayedCast()
    {
        yield return new WaitForSeconds(0.25f);
        Execute();

        yield return null;
    }
}
