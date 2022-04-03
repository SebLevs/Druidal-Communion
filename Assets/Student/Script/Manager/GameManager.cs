using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    public readonly float avrgTileSize = 0.32f;
    private int altarQtyMax;
    [Header("General parameters")]
    [SerializeField] private int altarQty = 0;
    [SerializeField] private Text altarQtyTxt;

    [Header("Game State")]
    [SerializeField] private GameObject wonOverRef;
    [SerializeField] private GameObject[] disableObjOnOver;
    [SerializeField] private Text canvastxt;
    [SerializeField] private float timerCharPrint;

    [Header("Sound")]
    [SerializeField] private AudioSource asPrint;

    private const string gameWonStr = "You have restored every shrine and saved the land from corruption.\n" +
                                                 "You are in communion with nature again.\n\n" +
                                                 "END OF THE DEMO\n" +
                                                 "THANK YOU FOR PLAYING";

    private const string gameOverStr = "You have failed to restore every shrine.\n\n"+
                                                  "Your connection to nature is ruptured and your life forfeit.\n\n" +
                                                  "GAME OVER"; 


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    #region SINGLETON
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        SetAltarQtyMaxStart();
        SetAltarQtyText();
    }

    // SECTION - Method - GUI Specific --------------------------------------------------------------------
    private void SetAltarQtyMaxStart()
    {
        GameObject[] altars = GameObject.FindGameObjectsWithTag("Altar");
        altarQtyMax = altars.Length;
    }

    private void SetAltarQtyText()
    {
        altarQtyTxt.text = altarQty.ToString("00") + $" / {altarQtyMax.ToString("00")}";
    }

    public void IncrementAltarQty() // Called inside [AltarClensing.cs] - OnRestored()
    {
        altarQty++;
        SetAltarQtyText();
        CheckAltarQtyEvent();
    }

    private void CheckAltarQtyEvent()
    {
        if (altarQty == altarQtyMax)
            GenericGameOver();
    }


    // SECTION - Method - Game Won / Over Specific --------------------------------------------------------------------
    public void GenericGameOver()
    {
        wonOverRef.SetActive(true);

        // Deactivate player to prevent enemies killing him if GAMEWON
        foreach (GameObject obj in disableObjOnOver)
            obj.SetActive(false);

        // Audiolistener of GameManager disabled by default as main AL on player
        GetComponent<AudioListener>().enabled = true;

        // GAMEWON
        if (altarQty == altarQtyMax)                    
            StartCoroutine(SimplePrintText(gameWonStr));
        // GAME OVER
        else
            StartCoroutine(SimplePrintText(gameOverStr));

    }

    private IEnumerator TimeStop()
    {
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            Time.timeScale -= (Time.timeScale - Time.deltaTime < 0.0f)? 0 : Time.deltaTime;
        } while (Time.timeScale > 0.2f);

        Time.timeScale = 0.0f;

        yield return null;
    }


    // TODO : Refactor with [SimpleIntroduction.cs] - [PrintText] into a single abstraction or override of an abstraction
    private IEnumerator SimplePrintText(string text)
    {
        char[] stringAsChar = text.ToCharArray();

        foreach (char character in stringAsChar)
        {
            canvastxt.text += character;
            asPrint.PlayOneShot(asPrint.clip);
            yield return new WaitForSeconds(timerCharPrint);
        }

        StartCoroutine(TimeStop());

        yield return null;
    }
}
