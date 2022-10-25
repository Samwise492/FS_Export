using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectAfterEvent : MonoBehaviour
{
    [SerializeField] private Transform spawnedObject;
    [SerializeField] private float x, y; // position of object's spawnpoint
    public FallingObject fallingObject;

    [HideInInspector] public bool activateButton, instantiateButton;

    [HideInInspector] public GameObject[] _objects;
    [HideInInspector] public Transform _object;
    [HideInInspector] public int count;

    private void Start()
    {
        StartCoroutine(RecreateEvent());
    }

    private void Update()
    {
        if (instantiateButton)
        {
            if (fallingObject.IsFallen)
            {
                _object = Instantiate(spawnedObject, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
        if (activateButton)
        {
            if (fallingObject.IsFallen)
                for (var i = 0; i < count; i++)
                {
                    _objects[i].SetActive(true);
                }                   
        }
    }
    IEnumerator RecreateEvent()
    {
        while (true)
        {
            if (Player.Instance.Health.CurrentHealth <= 0)
            {
                if (instantiateButton)
                    if (_object != null)
                    Destroy(_object.gameObject);
                if (activateButton)
                    for (var i = 0; i < count; i++)
                    {
                        _objects[i].SetActive(false);
                    }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
