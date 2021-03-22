using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private float max;
    [SerializeField] private float min;
    [SerializeField] private float speed;
    [SerializeField] Rigidbody2D rb;

    void Update()
    {
        if (transform.position.y <= min)
        {
            if (transform.position.y != max)
            {
                rb.velocity = Vector3.up;
                rb.velocity *= speed;
            }
        }
            
            
        if (transform.position.y >= max)
        {
            if (transform.position.y != min)
            {
                rb.velocity = Vector3.down;
                rb.velocity *= speed;
            }
        }
    }
}
