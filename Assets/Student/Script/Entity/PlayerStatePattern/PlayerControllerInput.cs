using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControllerInput : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    public AltarCleansing altarClensing;
    [SerializeField] private PlayerContext context;
    [SerializeField] private InteractCueManager icm;


    // SECTION - Method --------------------------------------------------------------------
    public void OnMove(InputAction.CallbackContext cbc)
    {
        context.InputDirH = cbc.ReadValue<Vector2>().x;
        context.InputDirV = cbc.ReadValue<Vector2>().y;
    }

    public void OnLook(InputAction.CallbackContext cbc)
    {
        if (Camera.main != null)
        {
            context.LookAt.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // cbc.ReadValue<Vector2>()
            context.LookAt.transform.rotation = context.InstantiateAt.transform.rotation; //Quaternion.identity
        }
    }

    public void OnCastMain(InputAction.CallbackContext cbc)
    {
        context.InputCastMain = cbc.performed;
    }

    public void OnCastSecondary(InputAction.CallbackContext cbc)
    {
        context.InputCastSecondary = cbc.performed;
    }

    public void OnCastSpace(InputAction.CallbackContext cbc)
    {
        context.InputCastSpace = cbc.performed;
    }

    public void OnOption(InputAction.CallbackContext cbc)
    {
        context.InputOption = cbc.performed;
    }

    public void OnInteract(InputAction.CallbackContext cbc)
    {
        icm.InputInteract = cbc.performed;
        if (altarClensing != null)
            altarClensing.InputInteract = cbc.performed;
    }
}
