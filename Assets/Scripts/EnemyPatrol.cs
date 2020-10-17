using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isRightDirection;
    #region speed
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > 0)
                speed = value;
        }
    }
    #endregion speed
    #region collisionTag
    [SerializeField] private string collisionTag;
    public string CollisionTag => collisionTag;
    #endregion collisionTag
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private CollisionDamage collisionDamage;

    private void Update()
    {
        animator.SetBool("isCollision", collisionDamage.IsCollision);

        if (groundDetection.IsGrounded)
            if (transform.position.x > rightBorder.transform.position.x || collisionDamage.Direction < 0)
                isRightDirection = false;
            else if (transform.position.x < leftBorder.transform.position.x || collisionDamage.Direction > 0)
                isRightDirection = true;

        rb.velocity = isRightDirection ? Vector2.right : Vector2.left;
        rb.velocity *= speed;

        if (rb.velocity.x < 0) // if speed less than 0
            spriteRenderer.flipX = false;
        if (rb.velocity.x > 0) 
            spriteRenderer.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
}
    
