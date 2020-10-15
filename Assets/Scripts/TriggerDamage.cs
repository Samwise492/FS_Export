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
            return; //выходим из метода не позволяя выполнять следующие строчки
         
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject)) //если контейнер в GameManager имеет ключ, равный объекту, с которым мы столкнулись
                                                                              //то есть есть ли у объекта скрипта Health
        {
            var health = GameManager.Instance.healthContainer[col.gameObject]; //получаем в переменную по ключу словаря компонент Health, ведь именно он записался в GameManager
            health.TakeHit(Damage);
        }
        if (isDestroyingAfterCollision)
        {
            if (destroyer == null) //если destroyer не сработал
                Destroy(gameObject);
            else destroyer.Destroy(gameObject); //далем уничтожение универсальным, не привязанным к методам юнити. Такое уничтожение нужно для разных типов объектов,
                                                //которые могут иметь своё уничтожение
        }       
    }
}

public interface IObjectDestroyer
{
    void Destroy(GameObject gameObject);
}
