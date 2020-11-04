using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGameObjectAfterEvent : MonoBehaviour
{
    private bool isInstantiate;
    [SerializeField] private Transform spawnedObject;
    [SerializeField] private float x, y; // position of object's spawnpoint
    [SerializeField] private FallingObject fallingObject;

    private void Update()
    {
        isInstantiate = fallingObject.IsFallen;
        if (fallingObject.IsFallen)
            Instantiate(spawnedObject, new Vector3(x, y, 0), Quaternion.identity);
    }
}
