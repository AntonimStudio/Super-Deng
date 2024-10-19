using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropDownResolution;
    [SerializeField] private TMP_Dropdown dropDownLanguage;
    //[SerializeField] private TMP_Dropdown qualityDropdown;

    [SerializeField] private TextMeshProUGUI textMusicVolume;
    [SerializeField] private Button buttonIncreaseMusic;    
    [SerializeField] private Button buttonDecreaseMusic;   

    private float currentVolume;
    private const int step = 5;        
    private const int minValue = 0;    
    private const int maxValue = 100;
    private Resolution[] resolutions;

    private void Start()
    {
        dropDownResolution.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

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
        Debug.Log(currentVolume.ToString());
        LoadSettings(currentResolutionIndex);
        Debug.Log(currentVolume.ToString());
    }
    /*
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        currentVolume = volume;
    }*/
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
            Debug.Log("1");
        }
            

        else
        {/*
            float value;
            if (audioMixer.GetFloat("MasterVolume", out value))
            {
                textMusicVolume.text = Mathf.Pow(10, value / 20).ToString(); ;  // ѕреобразуем обратно из dB в линейное значение
            }
            else
            {
                textMusicVolume.text = "100";
                currentVolume = 100;
                audioMixer.SetFloat("Master", Mathf.Log10(100) * 20);
                SetVolume(100);
            }*/
            textMusicVolume.text = "100";
            currentVolume = 100;
        }
            

    }
    public void IncreaseValue()
    {
        currentVolume = Mathf.Clamp(currentVolume + step, minValue, maxValue);
        UpdateText();

    }

    public void DecreaseValue()
    {
        currentVolume = Mathf.Clamp(currentVolume - step, minValue, maxValue);
        UpdateText();
    }

    private void UpdateText()
    {
        textMusicVolume.text = currentVolume.ToString();
    }
}