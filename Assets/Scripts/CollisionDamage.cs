using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    #region damage
    [SerializeField] private float damage = 10;
    public float Damage
    {
        get { return damage; }
        set
        {
            if (value > 0)
                damage = value;
        }
    }
    #endregion damage
    #region isCollision
    [SerializeField] private bool isCollision = false;
    public bool IsCollision => isCollision;
    #endregion isCollision;
    #region direction
    private float direction;
    public float Direction => direction;
    #endregion direction
    [SerializeField] private Animator animator;
    private Health health;

    private void OnCollisionEnter2D(Collision2D col) //отвечает за тот момент, когда только соприкоснулись объекты
    {
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            health = GameManager.Instance.healthContainer[col.gameObject];//в <> указываем компонент, который хотим получить от объекта, с которым столкнулись
                                                                              //в переменную health попал компонент Health того объекта, с которым мы столкнулись
            isCollision = true;
            direction = (col.transform.position - transform.position).x; //получаем вектор, показывающий направление
            animator.SetFloat("Direction", Mathf.Abs(direction));
        }
    }

    public void SetDamage() //use in animator as event
    {
        if (health != null)
            health.TakeHit(damage);
        else
            health = null;
        direction = 0; //если никуда не двигаемся, то и вектор направления равен 0
        animator.SetFloat("Direction", 0f);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        isCollision = false;
    }
}
