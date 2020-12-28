using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFading : MonoBehaviour
{
    private bool isTrigger;
    private float fadeValue = 1;
    [SerializeField] private Tilemap tilemap;
    private Color standartColor;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isTrigger = false;
        }
    }

    private void Start()
    {
        standartColor = tilemap.color; 
    }

    private void Update()
    {
        if (isTrigger) // fade the tilemap (change alpha value)
        {
            if (fadeValue > 0.025f)
                fadeValue -= 0.025f;
            tilemap.color = new Color(standartColor.r, standartColor.g, standartColor.b, fadeValue);
        }
        else // return alpha value to standart
        {
            if (fadeValue < 1)
                fadeValue += 0.025f;
            tilemap.color = new Color(standartColor.r, standartColor.r, standartColor.b, fadeValue);
        }
    }
}
