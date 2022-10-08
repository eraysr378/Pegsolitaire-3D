using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public Dropdown GraphicsDropdown;
    public Dropdown ResolutionDropdown;
    public Text graphicsText;
    public static int currentQualityIndex; // for each scene, quality settings should be configured
    private Resolution[] resolutions;
    private void Awake()
    {
        int qualityLevel = QualitySettings.GetQualityLevel();
        // choose appropriate graphics setting
        if (qualityLevel == 0)
        {
            GraphicsDropdown.value = 0;
        }
        else if(qualityLevel == 1)
        {
            GraphicsDropdown.value = 1;
        }
        else if(qualityLevel == 2)
        {
            GraphicsDropdown.value = 2;
        }
        else if(qualityLevel == 3)
        {
            GraphicsDropdown.value = 3;
        }
        else if (qualityLevel == 4)
        {
            GraphicsDropdown.value = 4;
        }
        else if (qualityLevel == 5)
        {
            GraphicsDropdown.value = 5;
        }
    }
    private void Start()
    {
        // put available resolutions into dropdown 
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(resolutionOptions); 
        ResolutionDropdown.value = currentResIndex;
        ResolutionDropdown.RefreshShownValue();
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        currentQualityIndex = qualityIndex;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }
}
