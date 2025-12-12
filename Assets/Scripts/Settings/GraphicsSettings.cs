using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownScreenSize;

    public TMP_Dropdown dpResolution;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        dpResolution.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dpResolution.AddOptions(options);
        dpResolution.value = currentResolutionIndex;
        dpResolution.RefreshShownValue();

        dropdownScreenSize.value = ModeToIndex(Screen.fullScreenMode);
        dropdownScreenSize.onValueChanged.AddListener(SetScreenMode);
    }

    private int ModeToIndex(FullScreenMode mode)
    {
        return mode switch
        {
            FullScreenMode.Windowed => 0,
            FullScreenMode.FullScreenWindow => 1,
            FullScreenMode.ExclusiveFullScreen => 2,
            _ => 0

        };
    }

    public void SetScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed; 
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow; 
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; 
                break;
        }
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
}
