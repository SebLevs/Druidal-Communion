using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // SECTION - Field --------------------------------------------------------------------
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private string exposedParam = "";
    const float defaultVol = 0.0f;
    private float minValue;


    // SECTION - Method --------------------------------------------------------------------
    private void Start()
    {
        // Set useful values
        GetComponent<Slider>().value = defaultVol;
        minValue = GetComponent<Slider>().minValue;

        // Set Volume
        SetVolume(defaultVol);
    }

    public void SetVolume(float vol)
    {
        // Teacher's version : In case of need
        //audioMixer.SetFloat(expsdParamStr, Mathf.Log10(vol) * 30);

        // Cut out sound when reaching minimal value accepted for slider
        // Otherwise, set sound normally
        if (vol <= minValue)
            audioMixer.SetFloat(exposedParam, -80.0f);
        else
            audioMixer.SetFloat(exposedParam, vol);
    }
}
