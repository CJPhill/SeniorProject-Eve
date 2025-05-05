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

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels")]
    public string  _newGameScene;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public GameManager gameManager;

    private void Start(){
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height){
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void NewGameDialogYes(){
        gameManager.sceneCall(_newGameScene);
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

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume){
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void setSenstivity(float sensitivity){
        sensTextValue.text = sensitivity.ToString("0.0");
    }

    public void setBrightness(float brightness){
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen){
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex){
        _qualityLevel = qualityIndex;
    }

    public void VolumeApply(){
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void GraphicsApply(){
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
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

    public void ResetButton(string menuType){
        if(menuType == "Graphics"){
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

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

    public IEnumerator ConfirmationBox(){
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
