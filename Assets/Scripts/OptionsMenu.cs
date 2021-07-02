using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private const string VolumeKey = "Volume";

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreen;
    public Toggle sound;
    public AudioMixer music;
    private Resolution[] resolutions;
    public void Start()
    {
        fullscreen.isOn = Screen.fullScreen;
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        var volume = PlayerPrefs.GetFloat(VolumeKey, 0);
        music.SetFloat(VolumeKey, volume);
        sound.isOn = volume == 0;

        LoadResolutions();
    }

    public void ToggleFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int index)
    {
        var resolution = resolutions[index];
        var maxRefreshRate = Screen.resolutions.Max(r => r.refreshRate);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, maxRefreshRate);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void ToggleSound(bool on)
    {
        var volume = on ? 0 : -80;
        music.SetFloat(VolumeKey, volume);
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    private void LoadResolutions()
    {
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions
        .Reverse()
        .Select(resolution => new Resolution
        {
            width = resolution.width,
            height = resolution.height
        })
        .Distinct()
        .ToArray();

        var resolutionOptions = new List<string>();
        var currentResolutionIndex = 0;
        for (var i = 0; i < resolutions.Length; i++)
        {
            var option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}