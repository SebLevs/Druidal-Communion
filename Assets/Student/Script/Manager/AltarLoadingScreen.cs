using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarLoadingScreen : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [Header("Particle System")]
    [SerializeField] private ParticleSystem psRestoredPref;
    [SerializeField] private ParticleSystem psCorrupted;
    private ParticleSystem.MainModule psMain;
    private Color psCleansingColor;
    private float psStartSpeed;


    // SECTION - Method - Unity Specific --------------------------------------------------------------------
    private void Start()
    {
        // Set Particle System fields
        psMain = psCorrupted.main;
        psCleansingColor = Color.white;
        psStartSpeed = psMain.startSpeedMultiplier;
    }


    // SECTION - Method - General --------------------------------------------------------------------
    public void OnRestored()
    {
        // Visual cues Initialization
        Instantiate(psRestoredPref, transform); // ps
        GetComponent<Animator>().SetBool("isAltarRestored", true); // Anim

        // Color Visual cues
        psMain.startColor = psCleansingColor;
        psMain.startSpeedMultiplier = psStartSpeed * -1;
    }
}
