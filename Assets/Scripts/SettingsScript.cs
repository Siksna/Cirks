using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;


public class SettingsScript : MonoBehaviour
{
    public AudioSource audioSource1; 
    public AudioSource audioSource2; 
    public Slider volumeSlider;

    public TMPro.TMP_Dropdown Resolution;

    Resolution[] resolutions;
  

    void Start()
    {

        //Resolutions
        resolutions = Screen.resolutions;

        Resolution.ClearOptions();

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


        Resolution.AddOptions(options);
        Resolution.value = currentResolutionIndex;
        Resolution.RefreshShownValue();


        //Audio


        if (volumeSlider != null && audioSource1 != null && audioSource2 != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            volumeSlider.value = audioSource1.volume;  
        }
    }

   

    //Resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Audio
    public void SetVolume(float volume)
    {
        if (audioSource1 != null)
            audioSource1.volume = volume;

        if (audioSource2 != null)
            audioSource2.volume = volume;
    }
}
