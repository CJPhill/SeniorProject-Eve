using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    [SerializeField] private Slider sensSlider = null;
    [SerializeField] private float defaultSens = 0.5f;
    public float mainSens = 0.5f;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertY = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels")]
    public string  _newGameScene;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    public void NewGameDialogYes(){
        SceneManager.LoadScene(_newGameScene);
    }

    public void LoadGameDialogYes(){
        if(PlayerPrefs.HasKey("SavedLevel")){
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else{
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void SetVolume(float volume){
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply(){
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string menuType){
        if(menuType == "Audio"){
            volumeSlider.value = defaultVolume;
            AudioListener.volume = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            PlayerPrefs.SetFloat("masterVolume", defaultVolume);
            VolumeApply();
        }
        if(menuType == "Gameplay"){
            sensTextValue.text = defaultSens.ToString("0.0");
            sensSlider.value = defaultSens;
            mainSens = defaultSens;

            invertY.isOn = false;
            GameApply();
        }
    }

    public void setSenstivity(float sensitivity){
        sensTextValue.text = sensitivity.ToString("0.0");
    }

    public void GameApply(){
        if(invertY.isOn){
            PlayerPrefs.SetInt("invertY", 1);
        }
        else{
            PlayerPrefs.SetInt("invertY", 0);
        }

        PlayerPrefs.SetFloat("masterSensitivity", mainSens);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox(){
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
