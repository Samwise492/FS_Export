﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
    // shader variables
    Material material;
    float fade = 1f;
    bool flag = false; // should object be faded
    [SerializeField] bool isPlayer;
    [SerializeField] bool isBoss;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material; // get a reference to the material
        GameManager.Instance.healthContainer.Add(gameObject, this); // transfer link on object and on needed element (this script)
    }

    public void TakeHit(float damage)
    {
        health -= damage;

        if (health <= 0 && !isPlayer)
        {
            flag = true;
            Update();
        }
        if (health <=0 && isPlayer)
            if (Player.Instance.BonusResurrection != 1)
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

                if (isPlayer)
                {
                    Player.Instance.Death();
                }
                if (isBoss)
                    return;
                if (!isPlayer && !isBoss)
                {
                    Destroy(gameObject);
                    //if (gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount <= 0)
                        //Destroy(gameObject);
                }
            }

            material.SetFloat("_Fade", fade);
            
        }     
    }

    public void SetHealth(float bonusHealth)
    {
        health += bonusHealth;

        if (health > 100)
            health = 100;
    }
}
