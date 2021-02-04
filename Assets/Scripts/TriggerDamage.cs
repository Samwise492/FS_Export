using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    #region damage
    [SerializeField] private float damage = 10;
    public float Damage
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
    public const float DefaultDamage = 10;

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

            if (isDestroyingAfterCollision)
            {
                // normal destroying
                if (destroyer == null) // if destroyer doesn't exist or somehow didn't work
                    Destroy(gameObject);
                // unique destroying - it isn't tied to Unity methods. 
                // such destroying is needed for various type of objects, 
                // which can have their own destroying
                else
                    destroyer.Destroy(gameObject);
            }
                
        }
        if (!GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            if (isDestroyingAfterCollision)
            {
                // normal destroying
                if (destroyer == null) // if destroyer doesn't exist or somehow didn't work
                    Destroy(gameObject);
                // unique destroying - it isn't tied to Unity methods. 
                // such destroying is needed for various type of objects, 
                // which can have their own destroying
                else
                    destroyer.Destroy(gameObject);
            }
        }
    }
}

public interface IObjectDestroyer
{
    void Destroy(GameObject gameObject);
}
