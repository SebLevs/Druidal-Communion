using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    // SECTION - Field -------------------------------------------------------------------
    [SerializeField] private bool isDestroyAtStart = true;
    [SerializeField] private float time;
                     private bool delegAllowDestr = false;


    // SECTION - Propriety -------------------------------------------------------------------
    public bool DelegAllowDestr { get => delegAllowDestr; set => delegAllowDestr = value; }


    // SECTION - Method -------------------------------------------------------------------
    private void Start()
    {
        if (isDestroyAtStart)
            Destroy(gameObject, time);
    }

    public void AEDestroyAtendAnim()
    {
        Destroy(gameObject);
    }

    private void AEDelegatedAllowDestroy()
    {
        if (delegAllowDestr)
            Destroy(gameObject);
    }

    public IEnumerator DelayedDetroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
