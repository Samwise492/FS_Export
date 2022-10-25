using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    static Transform startPosition;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallSpeed;
    #region isFallen
    private bool isFallen;
    public bool IsFallen => isFallen;
    #endregion

    private void Start()
    {
        startPosition = gameObject.transform;
        StartCoroutine(RecreateTrapAfterDeath());
    }

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
            rb.bodyType = RigidbodyType2D.Static;
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

    private IEnumerator RecreateTrapAfterDeath()
    {
        while (true)
        {
            if (Player.Instance.Health.CurrentHealth <= 0)
            {
                gameObject.transform.position -= new Vector3(0, startPosition.localPosition.y/3.5f, 0);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
