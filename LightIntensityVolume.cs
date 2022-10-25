using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightIntensityVolume : MonoBehaviour
{
    [SerializeField] Light2D lighting;
    [SerializeField] private float intensitySpeed;
    [SerializeField] private float maxInstensity;
    [SerializeField] private float minIntensity;
    private bool isIncreasing = true;

    void Update()
    {     
        if (isIncreasing)
        {
            lighting.intensity += intensitySpeed;
            if (lighting.intensity >= maxInstensity)
                isIncreasing = false;
        }
        if (!isIncreasing)
        {
            lighting.intensity -= intensitySpeed;
            if (lighting.intensity <= minIntensity)
                isIncreasing = true;
        }
    }
}
