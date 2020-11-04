using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallSpeed;
    #region isFallen
    private bool isFallen;
    public bool IsFallen => isFallen;
    #endregion


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.down * fallSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            rb.isKinematic = true;
            StartCoroutine(FallCheck());
        }
    }

    private IEnumerator FallCheck()
    {
        yield return 0;
        isFallen = true;
        yield return 0;
        isFallen = false;
        yield break;
    }
}
