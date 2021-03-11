using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightIncrease : MonoBehaviour
{
    [SerializeField] Light2D player_light;
    [SerializeField] float intensity; // varialbe which is used in dark places like cave
    [SerializeField] float escalationSpeed;
    static float defaultIntensity;
    [SerializeField] private bool increase;

    private void Start()
    {
        defaultIntensity = player_light.intensity;
    }

    private void Update()
    {
        if (increase)
            if (player_light.intensity < intensity)
                player_light.intensity += escalationSpeed;
        if (!increase)
            if (player_light.intensity > defaultIntensity)
                player_light.intensity -= escalationSpeed;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("FX"))
            increase = true;
            
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("FX"))
            //increase = false;
    }
}
