using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimpleIntroManager : MonoBehaviour
{
    /// <summary : How_To_Use>
    /// 
    /// How to use:
    ///     - Inspector text : | character for script's Replace()
    ///     - All arrays settup shoud be in sequential order of use
    /// 
    /// </summary>

    // SECTION - Field -------------------------------------------------------------------
    [Header("Object References")]
    [SerializeField] private GameObject[] objectsToToggle;
    [SerializeField] private Text canvasTxt;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [Tooltip("Targets in the desired order of camera focus")]
    [SerializeField] private Transform[] targetsInOrder;

    [Header("Text To print")]
    [SerializeField] private string[] textsToPrint;

    [Header("Timers")]
    [SerializeField] private float timerCharPrint;
    [SerializeField] private float timerLineSwitch;

    [Header("Sound")]
    [SerializeField] private AudioSource asPrint;


    // SECTION - Method - Unity Specific -------------------------------------------------------------------
    private void Start()
    {
        PrintAtFollowBehvavior();
    }


    // SECTION - Method - General -------------------------------------------------------------------
    private void ToggleAssets()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    private void PrintAtFollowBehvavior()
    {
        // Behavior explanation
        //      - Toggle and switch follow of CM at end of line print + N time 
        ToggleAssets();

        StartCoroutine(PrintText());
    }

    private void OnEnd()
    {
        cinemachine.Follow = GameObject.Find("Player").transform;

        // Garbage collector
        Destroy(canvasTxt.transform.root.gameObject);
        Destroy(GameObject.FindWithTag("CanvasIntro"));
        Destroy(GameObject.Find("INTRODUCTION"));
        GameObject.Find("Player").GetComponent<PlayerInput>().enabled = true; // Re-enable player input

        Cursor.visible = true;

        Destroy(this.gameObject);
    }


    // TODO - if time - : Refactor with [GameManager.cs] - [SimnplePrintText()] into a single abstraction or override of an abstraction
    private IEnumerator PrintText()
    {
        int targetIndex = 0;
        string lineReplaced;
        foreach (string line in textsToPrint)
        {
            cinemachine.Follow = targetsInOrder[targetIndex];
            lineReplaced = line.Replace('|', '\n');
            char[] stringAsChar = lineReplaced.ToCharArray();

            foreach (char character in stringAsChar)
            {
                canvasTxt.text += character;
                asPrint.PlayOneShot(asPrint.clip);
                yield return new WaitForSeconds(timerCharPrint);
            }

            yield return new WaitForSeconds(timerLineSwitch);
            canvasTxt.text = "";
            targetIndex++;
        }

        ToggleAssets();
        OnEnd();

        yield return null;
    }

    public void OnAnyKey(InputAction.CallbackContext cbc)
    {
        if (cbc.performed)
        {
            Cursor.visible = true;
            ToggleAssets();
            OnEnd();
        }
    }
}
