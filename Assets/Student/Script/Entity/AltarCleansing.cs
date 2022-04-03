using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary : How_To_Use>
/// 
///     - An enemyPrefab is spawned whenever spawnPoints are instantiated
///         + SpawnPoints are based off of a prefab ref for enemy spawn position
///         + SpawnPoints are autosufficient in their behaviors of instanciating ennemies 
///         
///     - altarCorrupMax must reach 0 for cleansing to succeed
///         + Corruption gradually grows back up whenever player stop cleasing
/// 
/// </summary>

public class AltarCleansing : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    private bool inputInteract = false;
    private bool areSpawnersInst = false;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem psRestoredPref;
    [SerializeField] private ParticleSystem psCorrupted;
                     private ParticleSystem.MainModule psMain;
                     private Color psCorruptColor;
                     private Color psCleansingColor;
                     private float psStartSpeed;

    [Header("Canvas")]
    [SerializeField] private GameObject corruptCanvas;
    [SerializeField] private Image corruptHp;
    [SerializeField] private Text corruptTxt;
    [SerializeField] private float maxCorruptValue = 10.0f;
                     private float currCorruptValue;

    private IObjectSpawner myObjectSpawner;
    private const string isAltarRestoredStr = "isAltarRestored";


    // SECTION - Field --------------------------------------------------------------------
    public bool InputInteract { get => inputInteract; set => inputInteract = value; }


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    private void Start()
    {
        myObjectSpawner = GetComponent<IObjectSpawner>();

        currCorruptValue = maxCorruptValue;
        corruptTxt.text = Math.Round(currCorruptValue, 1).ToString();

        // Set Particle System fields
        psMain = psCorrupted.main;
        psCorruptColor = psMain.startColor.color;
        psCleansingColor = Color.white;
        psStartSpeed = psMain.startSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        if (inputInteract) // Corruption cleansing
        {
            if (!areSpawnersInst)
            {
                corruptCanvas.SetActive(true);
                InstantiateSpawnPoints();
            }
            else
                RefreshCooldown();

            if (psMain.startSpeedMultiplier != psStartSpeed * -1)
            {
                psMain.startColor = psCleansingColor;
                psMain.startSpeedMultiplier = psStartSpeed * -1;
            }
        }
        else if (currCorruptValue != maxCorruptValue) // corruption takes over
            CorruptionGrowingBack();
    }

    #region SET ALTAR CLENSING INSTANCE ON PLAYER
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set altar clensing instance on player input for input feedback
        if (other.gameObject.CompareTag("Player") && this.currCorruptValue != 0.0f)
            other.gameObject.GetComponent<PlayerControllerInput>().altarClensing = this;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControllerInput>().altarClensing = null;
            inputInteract = false;
        }
    }
    #endregion


    // SECTION - Method - General --------------------------------------------------------------------
    private void InstantiateSpawnPoints()
    {
        areSpawnersInst = true;
        myObjectSpawner.Execute();
    }

    private void OnRestored()
    {
        // Not interactible anymore
        gameObject.layer = 0;

        // Visual cues
        Instantiate(psRestoredPref, transform);
        GetComponent<Animator>().SetBool(isAltarRestoredStr, true);
        GameManager.instance.IncrementAltarQty();

        // Set [Animator]
        //      - Altar
        myObjectSpawner.GetThisObject().GetComponent<Animator>().SetBool(isAltarRestoredStr, true);
        //      - Spawners & their respective list of spawnables
        foreach (GameObject spawner in myObjectSpawner.GetMySpawnableList())
        {
            spawner.GetComponent<Animator>().SetBool(isAltarRestoredStr, true);
            List<GameObject> mySpawnableList = spawner.GetComponent<SimpleObjectSpawner>().GetMySpawnableList();
            foreach (GameObject spawnable in mySpawnableList)
                if (spawnable != null)
                    spawnable.GetComponent<Animator>().SetBool(isAltarRestoredStr, true);
        }

        // Destroy Canvas & Script : Those are Not needed anymore
        //      - Enemies will destroy themselves on death as Animator.isAltarRestored == triggered
        Destroy(corruptCanvas);

        Destroy(this);
    }

    private void RefreshCooldown()
    {
        if (currCorruptValue > 0.0f)
        {
            currCorruptValue -= Time.deltaTime;
            corruptTxt.text = Math.Round((currCorruptValue), 1).ToString(); // Set TEXT
            corruptHp.fillAmount = currCorruptValue / maxCorruptValue; // set FILLER MASK
        }
        else // Altart Restored, yay!
            OnRestored();
    }

    private void CorruptionGrowingBack()
    {
        // Set particle system
        // https://docs.unity3d.com/ScriptReference/ParticleSystem.MainModule-startSpeed.html
        if (psMain.startSpeedMultiplier != psStartSpeed)
        {
            psMain.startSpeedMultiplier = psStartSpeed; // startSpeed
            psMain.startColor = psCorruptColor; // startColor
        }

        currCorruptValue += Time.deltaTime;

        if (currCorruptValue > maxCorruptValue)
            currCorruptValue = maxCorruptValue;

        corruptTxt.text = Math.Round((currCorruptValue), 1).ToString(); // Set TEXT
        corruptHp.fillAmount = currCorruptValue / maxCorruptValue; // set FILLER MASK
    }
}
