using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTriggerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var health = GameManager.Instance.healthContainer[col.gameObject];
            health.TakeHit(100);
        }          
    }
}
