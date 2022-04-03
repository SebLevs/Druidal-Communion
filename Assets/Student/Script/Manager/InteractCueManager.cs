using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractCueManager : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
                     private bool inputInteract = false;
    [SerializeField] private GameObject canvasInteract;
    [SerializeField] private Image interactBckg;
    [SerializeField] private Sprite[] avblInteractBckg;
    [SerializeField] private Image interactIcon;
    [SerializeField] private Sprite[] avblInteractIcon;
    [SerializeField] private LayerMask mask;


    // SECTION - Propriety --------------------------------------------------------------------
    public bool InputInteract { set => inputInteract = value; }


    // SECTION - Method --------------------------------------------------------------------
    private void Start()
    {
        // Tick less frequently than FixedUpdate. Yay!?
        InvokeRepeating("OnOverLapCircleTrigger", 0.0f, 0.20f);
    }

    private void FixedUpdate()
    {
        // TODO : Refactorise to implement gamepad - If gamepad cursor movement is implemented -
        if (inputInteract && canvasInteract.activeSelf)
        {
            interactBckg.GetComponent<Image>().sprite = avblInteractBckg[1];
            interactIcon.GetComponent<Image>().sprite = avblInteractIcon[1];
        }
        else if (!inputInteract && canvasInteract.activeSelf)
        {
            interactBckg.GetComponent<Image>().sprite = avblInteractBckg[0];
            interactIcon.GetComponent<Image>().sprite = avblInteractIcon[0];
        }
    }

    private void OnOverLapCircleTrigger()
    {
        if (IsTouchingLayer())
        {
            canvasInteract.SetActive(true);
        }
        else if (!IsTouchingLayer() && canvasInteract.activeSelf)
            canvasInteract.SetActive(false);
    }

    private bool IsTouchingLayer()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, GameManager.instance.avrgTileSize * 0.5f, mask);

        return col != null;
    }

}
