using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SimpleLoadScene : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    private AsyncOperation asyncLoad;
    private bool readyToLoadNext = false;
    private bool inputAnyKey = false;

    [Header("Progress Bar")]
    [SerializeField] private Image progressBar;
    [SerializeField] private Text progressText;

    [Header("Visual Cues")]
    [SerializeField] GameObject pressAnyKey;
    private AltarLoadingScreen altar;


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    private void Start()
    {
        Cursor.visible = false;
        InitialiseSettings();
    }

    private void Update()
    {
        UpdateFillerAndText();
        LoadNextScene();
    }


    // SECTION - Method - General --------------------------------------------------------------------
    private void InitialiseSettings()
    {
        // Set Altar visual cue ref
        altar = GameObject.Find("Altar Loading Screen").GetComponent<AltarLoadingScreen>();

        // Async load the next scene
        asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoad.allowSceneActivation = false; // Prevents instantaneous LoadScene()

        // Prevents unintentional inputs
        Input.ResetInputAxes();

        // Garbage collection - just in case
        System.GC.Collect();

        // Set base fill amount to fake progress
        progressBar.fillAmount = 0.15f;
    }

    private void UpdateFillerAndText()
    {
        if (progressText)
        {
            progressBar.fillAmount = asyncLoad.progress + 0.1f;
            progressText.text = (asyncLoad.progress * 100 + 10).ToString("f2") + " %";
        }


        if (progressBar.fillAmount == 1.0f)
            ActivateCompletionCues();
    }

    private void ActivateCompletionCues()
    {
        if (!pressAnyKey.activeSelf)
        {
            // Visual cues
            altar.OnRestored();
            pressAnyKey.SetActive(true);

            // Allows LoadScene()
            readyToLoadNext = true;
        }
    }

    private void LoadNextScene()
    {
        if (readyToLoadNext && inputAnyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnAnyKey(InputAction.CallbackContext cbc)
    {
        inputAnyKey = cbc.performed;
    }
}
