using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Health : MonoBehaviour
{
    #region health
    [SerializeField] private float health;
    public float CurrentHealth
    {
        get { return health; }
        set
        {
            if (value >= 0)
                health = value;
        }
    } //public Health
    #endregion health
    //shader variables
    Material material;
    float fade = 1f;
    bool flag = false;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material; //Get a reference to the material
        GameManager.Instance.healthContainer.Add(gameObject, this); //передаем ссылку на объект и на нужный элемент (этот скрипт)
    }

    public void TakeHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            flag = true;
            Update();
        }

        
    }

    private void Update()
    {
        if (!flag)
            enabled = false;
        else
        {
            enabled = true;

            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                Destroy(gameObject);
            }

            material.SetFloat("_Fade", fade);
            
        }
        
    }

    public void SetHealth(int bonusHealth)
    {
        health += bonusHealth; //this.health += health; this - Обращаемся к переменной, которая находится в теле скрипта, а не в методе

        if (health > 100)
            health = 100;
    }

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Heal"))
        {
            health += 40;
            Destroy(col.gameObject);

            if (health > 100)
                health = 100;
        }
    }*/
}
