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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject)) // if type of received object equals to object which should be stored in our container, we add it
        {
            health = GameManager.Instance.healthContainer[col.gameObject];
            isCollision = true;
            direction = (col.transform.position - transform.position).x; // recieve vector, which show direction
            animator.SetFloat("Direction", Mathf.Abs(direction));
        }

        DealDamage();
    }

    public void DealDamage() 
    {
        if (health != null)
            health.TakeHit(damage);
        else
            health = null;
        direction = 0; // if we aren't moving, then vector of direction equals 0
        animator.SetFloat("Direction", 0f);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        isCollision = false;
    }

    
}
