using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;
    
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;  
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality Level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    [SerializeField] private Slider sensSlider = null;

    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertY = null;

    private void Awake(){
        if(canUse){
            if(PlayerPrefs.HasKey("masterVolume")){
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }

            else{
                menuController.ResetButton("Audio");   
            }

            if(PlayerPrefs.HasKey("masterQuality")){
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if(PlayerPrefs.HasKey("masterFullScreen")){
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullScreen == 1){
                    fullScreenToggle.isOn = true;
                    Screen.fullScreen = true;
                }

                else{
                    fullScreenToggle.isOn = false;
                    Screen.fullScreen = false;
                }
            }

            if(PlayerPrefs.HasKey("masterBrightness")){
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }

            if(PlayerPrefs.HasKey("masterSensitivity")){
                float localSensitivity = PlayerPrefs.GetFloat("masterSensitivity");

                sensTextValue.text = localSensitivity.ToString("0.0");
                sensSlider.value = localSensitivity;
            }

            if(PlayerPrefs.HasKey("invertY")){
                int localInvertY = PlayerPrefs.GetInt("invertY");

                if(localInvertY == 1){
                    invertY.isOn = true;
                }

                else{
                    invertY.isOn = false;
                }
            }
        }
    }

}
