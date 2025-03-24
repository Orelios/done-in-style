using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0, savedResolutionIndex, savedWidth, savedHeight;

    void Start()
    {
        if (PlayerPrefs.HasKey("resWidth") && PlayerPrefs.HasKey("resHeight"))
        {
            savedWidth = PlayerPrefs.GetInt("resWidth");
            savedHeight = PlayerPrefs.GetInt("resHeight");
        }

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        filteredResolutions.Sort((a, b) => {
            if (a.width != b.width)
                return b.width.CompareTo(a.width);
            else
                return b.height.CompareTo(a.height);
        });

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value.ToString("0.##") + " Hz"; // Ondal?k basamak s?n?rland?
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height && (float)filteredResolutions[i].refreshRateRatio.value == currentRefreshRate) // double'dan float'a dönü?türüldü
            {
                currentResolutionIndex = i;
            }

            if (filteredResolutions[i].width == savedWidth && filteredResolutions[i].height == savedHeight && (float)filteredResolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                savedResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        

        
        //SetResolution(currentResolutionIndex);

        if (PlayerPrefs.HasKey("resWidth") && PlayerPrefs.HasKey("resHeight"))
        {
            resolutionDropdown.value = savedResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            Screen.SetResolution(savedWidth, savedHeight, true);
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(currentResolutionIndex);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        PlayerPrefs.SetInt("resWidth", resolution.width);
        PlayerPrefs.SetInt("resHeight", resolution.height);
    }
}