using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonEventManager : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private Selectable selectable;
    [SerializeField] private AudioSource asDeselect;
    [SerializeField] private float volumeScale = 0.5f;


    // SECTION - Method -------------------------------------------------------------------- 
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();      
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectable.OnPointerExit(null);
        asDeselect.PlayOneShot(asDeselect.clip, volumeScale);
    }    
}
