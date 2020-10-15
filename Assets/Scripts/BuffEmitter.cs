using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEmitter : MonoBehaviour //give buffs
{
    [SerializeField] private Buff buff;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.buffRecieverContainer.ContainsKey(col.gameObject))
        {
            var reciever = GameManager.Instance.buffRecieverContainer[col.gameObject];
            reciever.AddBuff(buff); //add received buff in our list of buffs
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (GameManager.Instance.buffRecieverContainer.ContainsKey(col.gameObject))
        {
            var reciever = GameManager.Instance.buffRecieverContainer[col.gameObject];
            reciever.RemoveBuff(buff); //add received buff in our list of buffs
        }
    }
}

[System.Serializable] //делаем поля доступные для сериализации
public class Buff
{
    public BuffType type;
    public int bonus;
}

public enum BuffType : byte
{
    Damage, Force, Health
}