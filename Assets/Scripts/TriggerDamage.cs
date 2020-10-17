using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    #region damage
    [SerializeField] private int damage = 10;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    #endregion
    #region parent
    private GameObject parent;
    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }
    #endregion
    [SerializeField] private bool isDestroyingAfterCollision;
    private IObjectDestroyer destroyer;
    public const int DefaultDamage = 10;

    public void Init(IObjectDestroyer destroyer)
    {
        this.destroyer = destroyer;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == parent)
            return; // exit from method. It doesn't allow to do other strings of code
         
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            var health = GameManager.Instance.healthContainer[col.gameObject];
            health.TakeHit(Damage);
        }
        if (isDestroyingAfterCollision)
        {
            if (destroyer == null) // if destroyer didn't work
                Destroy(gameObject);
            else destroyer.Destroy(gameObject); // make destroying unique, it isn't tie to Unity methods. Such destroying is needed for various type of objects, which can have their own destroying
        }       
    }
}

public interface IObjectDestroyer
{
    void Destroy(GameObject gameObject);
}
