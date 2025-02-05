using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;


public class BrightnessScript : MonoBehaviour
{
 
    public Slider BrightnessSlider;

    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    AutoExposure exposure;

    void Start()
    {
        //Brightness
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(BrightnessSlider.value);


      
    }

    //Brightness
    public void AdjustBrightness(float value)
    {
        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = .05f;
        }
    }

  
}

