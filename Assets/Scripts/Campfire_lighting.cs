using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Campfire_lighting : MonoBehaviour
{
    [SerializeField] Light2D lighting;
    [SerializeField] private float intensitySpeed;
    private bool isIncreasing = true;

    void Update()
    {
        
        if (isIncreasing)
        {
            lighting.intensity += intensitySpeed;
            if (lighting.intensity >= 2)
                isIncreasing = false;
        }
        if (!isIncreasing)
        {
            lighting.intensity -= intensitySpeed;
            if (lighting.intensity <= 1)
                isIncreasing = true;
        }
    }
}
