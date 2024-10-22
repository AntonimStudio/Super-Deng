using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle toggleFullScreen;
    [SerializeField] private TMP_Dropdown dropDownResolution;
    [SerializeField] private TMP_Dropdown dropDownLanguage;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private MovementButtonsChanger MBC;
    [Header("MasterVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textMasterVolume;
    [SerializeField] private Button buttonIncreaseMaster;
    [SerializeField] private Button buttonDecreaseMaster;
    [SerializeField] private string volumeParameterMaster = "MasterVolume";
    private float currentMasterVolume = 100;
    [Header("MusicVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textMusicVolume;
    [SerializeField] private Button buttonIncreaseMusic;    
    [SerializeField] private Button buttonDecreaseMusic;
    [SerializeField] private string volumeParameterMusic = "MusicVolume";
    private float currentMusicVolume = 100;
    [Header("SFXVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textSFXVolume;
    [SerializeField] private Button buttonIncreaseSFX;
    [SerializeField] private Button buttonDecreaseSFX;
    [SerializeField] private string volumeParameterSFX = "SFXVolume";
    private float currentSFXVolume = 100;

    private const int step = 5;        
    private const float minValue = 0;    
    private const int maxValue = 100;
    private int easterEggCounter = 0;
    public int parameter;
    private Resolution[] resolutions;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        dropDownResolution.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        easterEggCounter = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; // + " " + resolutions[i].refreshRateRatio + "Hz"
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        dropDownResolution.AddOptions(options);
        dropDownResolution.RefreshShownValue();

        buttonIncreaseMaster.onClick.AddListener(() => IncreaseValue(ref currentMasterVolume, volumeParameterMaster, textMasterVolume));
        buttonDecreaseMaster.onClick.AddListener(() => DecreaseValue(ref currentMasterVolume, volumeParameterMaster, textMasterVolume));
        UpdateText(currentMasterVolume,volumeParameterMaster, textMasterVolume);

        buttonIncreaseMusic.onClick.AddListener(() => IncreaseValue(ref currentMusicVolume, volumeParameterMusic, textMusicVolume));
        buttonDecreaseMusic.onClick.AddListener(() => DecreaseValue(ref currentMusicVolume, volumeParameterMusic, textMusicVolume));
        UpdateText(currentMusicVolume,volumeParameterMusic, textMusicVolume);

        buttonIncreaseSFX.onClick.AddListener(() => IncreaseValue(ref currentSFXVolume, volumeParameterSFX, textSFXVolume));
        buttonDecreaseSFX.onClick.AddListener(() => DecreaseValue(ref currentSFXVolume, volumeParameterSFX, textSFXVolume));
        UpdateText(currentSFXVolume, volumeParameterSFX, textSFXVolume);

        LoadSettings();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("LanguageSettingPreference", dropDownLanguage.value);
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", dropDownResolution.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MasterVolumePreference", currentMasterVolume);
        PlayerPrefs.SetFloat("MusicVolumePreference", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolumePreference", currentSFXVolume);
        MBC.SaveSettings();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("LanguageSettingPreference"))
            dropDownLanguage.value = PlayerPrefs.GetInt("LanguageSettingPreference");
        else
            dropDownLanguage.value = 0;

        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 1;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            dropDownResolution.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    dropDownResolution.value = i;
            }
        }


        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
            toggleFullScreen.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
            toggleFullScreen.isOn = true;
        }


        if (PlayerPrefs.HasKey("MasterVolumePreference"))
        {
            currentMasterVolume = PlayerPrefs.GetFloat("MasterVolumePreference");

            if (currentMasterVolume == 0)
            {
                audioMixer.SetFloat(volumeParameterMaster, DecibelConvert(0.00001f));
                textMasterVolume.text = "0";
            }
            else
            {
                textMasterVolume.text = PlayerPrefs.GetFloat("MasterVolumePreference").ToString();
                var value = DecibelConvert(PlayerPrefs.GetFloat("MasterVolumePreference"));
                audioMixer.SetFloat(volumeParameterMaster, value);
            }
        }
        else
        {
            audioMixer.SetFloat(volumeParameterMaster, DecibelConvert(100));
            textMasterVolume.text = "100";
            currentMasterVolume = 100;
        }

        if (PlayerPrefs.HasKey("MusicVolumePreference"))
        {
            currentMusicVolume = PlayerPrefs.GetFloat("MusicVolumePreference");

            if (currentMusicVolume == 0)
            {
                audioMixer.SetFloat(volumeParameterMusic, DecibelConvert(0.00001f));
                textMusicVolume.text = "0";
            }
            else
            {
                textMusicVolume.text = PlayerPrefs.GetFloat("MusicVolumePreference").ToString();
                var value = DecibelConvert(PlayerPrefs.GetFloat("MusicVolumePreference"));
                audioMixer.SetFloat(volumeParameterMusic, value);
            }
        }
        else
        {
            audioMixer.SetFloat(volumeParameterMusic, DecibelConvert(100));
            textMusicVolume.text = "100";
            currentMusicVolume = 100;
        }

        if (PlayerPrefs.HasKey("SFXVolumePreference"))
        {
            currentSFXVolume = PlayerPrefs.GetFloat("SFXVolumePreference");
            
            if (currentSFXVolume == 0)
            {
                audioMixer.SetFloat(volumeParameterSFX, DecibelConvert(0.00001f));
                textSFXVolume.text = "0";
            }
            else
            {
                textSFXVolume.text = PlayerPrefs.GetFloat("SFXVolumePreference").ToString();
                var value = DecibelConvert(PlayerPrefs.GetFloat("SFXVolumePreference"));
                audioMixer.SetFloat(volumeParameterSFX, value);
            }
        }
        else
        {
            audioMixer.SetFloat(volumeParameterSFX, DecibelConvert(100));
            textSFXVolume.text = "100";
            currentSFXVolume = 100;
        }
        MBC.LoadSettings();
    }
    private float DecibelConvert(float volumeValue)
    {
        var value = Mathf.Log10(volumeValue) * parameter - 45;
        return value;
    }

    public void IncreaseValue(ref float currentVolume, string volumeParameter, TextMeshProUGUI text)
    {
        currentVolume = Mathf.Clamp(currentVolume + step, minValue, maxValue);

        if (currentVolume == 100)
        {
            easterEggCounter += 1;
            if (easterEggCounter >= 15)
            {
                currentVolume = 999;
            }
        }
        UpdateText(currentVolume, volumeParameter, text);
    }

    public void DecreaseValue(ref float currentVolume, string volumeParameter, TextMeshProUGUI text)
    {
        easterEggCounter = 0;
        currentVolume = Mathf.Clamp(currentVolume - step, minValue, maxValue);
        if (currentVolume == 0)
        {
            audioMixer.SetFloat(volumeParameter, DecibelConvert(0.00001f));
            text.text = "0";
        }
        else
            UpdateText(currentVolume, volumeParameter, text);
    }

    public void UpdateText(float currentVolume, string volumeParameter, TextMeshProUGUI text)
    {
        audioMixer.SetFloat(volumeParameter, DecibelConvert(currentVolume));
        text.text = currentVolume.ToString();
    }
}