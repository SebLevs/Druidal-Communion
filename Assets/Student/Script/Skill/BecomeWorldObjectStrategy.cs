using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BecomeWorldObjectStrategy : MonoBehaviour
{
    /// <summary : How_To_Use>
    /// 
    ///     - Add as component to desired object
    ///     - Cooldown may be manually set
    ///     
    ///     - AEBecomeWO() & isWoAtStart boolean
    ///         + If iswoAtStart == false, AEBecomeWo() should be implemented at desired animation
    /// 
    /// </summary>


    // SECTION - Method --------------------------------------------------------------------
    private const string tagAtEndAnim = "WorldObject";
    [SerializeField] private Animator anim;
    [SerializeField] private bool isWoAtStart = false;

    [Header("CoolDown parameters")]
    [SerializeField] private float lifeTimerMax;
                     private float currLifeTimer = 0;


    // SECTION - Method --------------------------------------------------------------------
    private void Start()
    {
        if (isWoAtStart)
           StartCoroutine(StartLifeTimer());
    }

    private IEnumerator StartLifeTimer()
    {
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);

            currLifeTimer += Time.deltaTime;
        } while (currLifeTimer < lifeTimerMax);

        OnLifeTimerEnd();
        yield return null;
    }

    private void OnLifeTimerEnd()
    {
        anim.SetBool("isDead", true);
    }
    private void AEBecomeWO() // Animation Event
    {
        gameObject.tag = tagAtEndAnim;
        gameObject.layer = LayerMask.NameToLayer(tagAtEndAnim);//maskAtEndAnim;
        StartCoroutine(StartLifeTimer());
    }

    private void AEDestroyWO() // Animation Event
    {
        Destroy(gameObject);
    }
}
