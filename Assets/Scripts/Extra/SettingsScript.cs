using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropDownResolution;
    [SerializeField] private TMP_Dropdown dropDownLanguage;
    //[SerializeField] private TMP_Dropdown qualityDropdown;

    [SerializeField] private TextMeshProUGUI textMusicVolume;
    [SerializeField] private Button buttonIncreaseMusic;    
    [SerializeField] private Button buttonDecreaseMusic;

    [SerializeField] private string volumeParameter = "MusicVolume";
    private float currentVolume;
    private const int step = 5;        
    private const float minValue = 0;    
    private const int maxValue = 100;
    private int easterEggCounter = 0;
    public int parameter;
    private Resolution[] resolutions;

    private void Start()
    {
        PlayerPrefs.DeleteAll();

        dropDownResolution.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        easterEggCounter = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; // + " " + resolutions[i].refreshRateRatio + "Hz"
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        dropDownResolution.AddOptions(options);
        dropDownResolution.RefreshShownValue();

        buttonIncreaseMusic.onClick.AddListener(IncreaseValue);
        buttonDecreaseMusic.onClick.AddListener(DecreaseValue);
        UpdateText();
        
        LoadSettings(currentResolutionIndex);
        
    }
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * parameter);
        currentVolume = volume;
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
        //PlayerPrefs.SetInt("QualitySettingPreference",
                   //qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference",
                   dropDownResolution.value);
        PlayerPrefs.SetInt("FullscreenPreference",
                   System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MusicVolumePreference", currentVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        /*if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value =
                         PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;*/
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            dropDownResolution.value =
                         PlayerPrefs.GetInt("ResolutionPreference");
        else
            dropDownResolution.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen =
            System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("MusicVolumePreference"))
        {
            textMusicVolume.text = PlayerPrefs.GetFloat("MusicVolumePreference").ToString();

            var value = DecibelConvert(PlayerPrefs.GetFloat("MusicVolumePreference"));
            audioMixer.SetFloat(volumeParameter, value);
        }
        else
        {
            audioMixer.SetFloat(volumeParameter, DecibelConvert(100));
            textMusicVolume.text = "100";
            currentVolume = 100;
        }
            

    }
    private float DecibelConvert(float volumeValue)
    {
        var value = Mathf.Log10(volumeValue) * parameter - 45;
        return value;
    }

    public void IncreaseValue()
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
        UpdateText();
    }

    public void DecreaseValue()
    {
        easterEggCounter = 0;
        currentVolume = Mathf.Clamp(currentVolume - step, minValue, maxValue);
        if (currentVolume == 0)
        {
            audioMixer.SetFloat(volumeParameter, DecibelConvert(0.00001f));
            textMusicVolume.text = "0";
        }
        else
            UpdateText();
    }

    private void UpdateText()
    {
        
        audioMixer.SetFloat(volumeParameter, DecibelConvert(currentVolume));
        textMusicVolume.text = currentVolume.ToString();
    }
}