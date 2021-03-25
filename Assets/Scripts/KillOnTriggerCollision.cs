using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTriggerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("caught");
            var health = GameManager.Instance.healthContainer[col.gameObject];
            Debug.Log("was " + health.CurrentHealth);
            health.CurrentHealth -= 500;
            Debug.Log("and now is " + health.CurrentHealth);
        }
            
    }
}
