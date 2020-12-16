using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTransparencyController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Trigger triggerPoint;

    private void Update()
    {
        if (triggerPoint.isTrigger)
            RevealTilemap();
    }

    public void RevealTilemap()
    {
        float transparency = tilemap.GetComponent<Color>().a;
        transparency = 50;
    }
}
