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
        animator.SetBool("isCollision", collisionDamage.IsCollision); //принимаем состояние переменной isCollision из скрипта CollisionDamage, это нужно для аниматора

        if (groundDetection.IsGrounded) //если он находится на земле, то... //это не позволяет двигаться объекту если он не на земле
            if (transform.position.x > rightBorder.transform.position.x || collisionDamage.Direction < 0) //если у enemy переменная x компонента transform больше, 
                                                                            //чем переменная x компонента transform правой границы или направление меньше нуля, то
                isRightDirection = false;
            else if (transform.position.x < leftBorder.transform.position.x || collisionDamage.Direction > 0)
                isRightDirection = true;

        rb.velocity = isRightDirection ? Vector2.right : Vector2.left; //если isRightDirection = true, то в rb.velocity запишется Vector2.right, иначе - Vector2.left
        rb.velocity *= speed;

        if (rb.velocity.x < 0) //если скорость больше 0
            spriteRenderer.flipX = false; //отменяем эффект отражения
        if (rb.velocity.x > 0)
            spriteRenderer.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); //устанавливаем переменной Speed типа float значение равное скорости; Mathf.Abs возвращает модуль от числа
    }
}
    
