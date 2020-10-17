using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallingFromTrigger : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject fallingObject; // object that falls
    [SerializeField] private GameObject triggerPoint; // spot for triggering
    [SerializeField] private Transform fallPoint; // spot where object falls

    private void Update()
    {
        if (player.transform.position.x >= triggerPoint.transform.position.x)
            Falling();
    }

    private void Falling()
    {
        Instantiate(fallingObject, fallPoint);
        //fallingObject.transform.position.y -= 0.1;
    }
}
