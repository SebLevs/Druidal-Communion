using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class OptionManager : MonoBehaviour
{
    // SECTION - Field -------------------------------------------------------------------
    private bool isGamePaused = false;

    [SerializeField] private bool isMainMenu = false;
    [SerializeField] private PlayerContext playerContext;

    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private Selectable mainSelectable;

    [Header("Sound")]
    [SerializeField] private AudioSource asValidation;
    [SerializeField] private float volumeScale = 0.5f;
                     private const float silence = 0.05f;


    // SECTION - Method -------------------------------------------------------------------
    #region SINGLETON
    public static OptionManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        if (isMainMenu)
            mainSelectable.Select();
    }

    private void Update()
    {
        if (!isMainMenu)
            if (playerContext.InputOption) // Toggle Option Canvas
            {       
                isGamePaused = !isGamePaused;

                playerContext.InputOption = false;

                // Event System don't select properly otherwhise
                if (!optionCanvas.activeSelf)
                    EventSystem.current.SetSelectedGameObject(null);

                // isGamePaused == Player can go on another screen
                Cursor.lockState = (isGamePaused)? CursorLockMode.None : CursorLockMode.Confined;
                // isGamePaused == Freeze game physics
                Time.timeScale = (isGamePaused) ? 0.0f : 1.0f;

                optionCanvas.SetActive(!optionCanvas.activeSelf);

                if (optionCanvas.activeSelf)
                    mainSelectable.Select();
            }
    }

    public void StartGame()
    {
        // Restore timescale
        Time.timeScale = 1.0f;
        StartCoroutine(WaitForClipEnd("Start"));
    }

    public void RestartLevel() // Button Click Event Call
    {
        // Restore timescale
        Time.timeScale = 1.0f;
        StartCoroutine(WaitForClipEnd("Restart"));   
    }

    public void QuitGame() // Button Click Event Call
    {
        Time.timeScale = 1.0f;
        StartCoroutine(WaitForClipEnd("Quit"));      
    }

    private IEnumerator WaitForClipEnd(string callMethod)
    {
        asValidation.PlayOneShot(asValidation.clip, volumeScale);
        yield return new WaitForSeconds(asValidation.clip.length + silence);

        switch (callMethod) // Not the cleanest way, but does the job
        {
            case "Quit":
                Application.Quit();
                break;
            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Start":
                int scene = (SceneManager.GetActiveScene().buildIndex == 0) ? 1 : 0;
                SceneManager.LoadScene(scene);
                break;
            default: Debug.Log("An error has occured at [OptionManager.cs] - [WaitForClipEnd]"); break;

        }

        yield return null;
    }
}
