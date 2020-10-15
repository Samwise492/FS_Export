﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffReciever : MonoBehaviour //recieve buffs, add to player
{
    #region buffs
    private List<Buff> buffs;
    public List<Buff> Buffs => buffs;
    #endregion
    public Buff recievedBuff;
    public Action OnBuffsChanged;

    private void Start()
    {
        GameManager.Instance.buffRecieverContainer.Add(gameObject, this); //put buff in our container
        buffs = new List<Buff>(); //create buff list
    }

    public void AddBuff(Buff buff)
    {
        if (!buffs.Contains(buff)) //if current buffs don't contain new buff which we want to add (in order to don't add the same buff)
            buffs.Add(buff); //add this buff

        if (OnBuffsChanged != null) // test
            OnBuffsChanged();

        recievedBuff = buff;
    }

    public void RemoveBuff(Buff buff)
    {
        if (buffs.Contains(buff)) //if our buff list has exact buff
            buffs.Remove(buff); //remove this buff

        if (OnBuffsChanged != null) // test
            OnBuffsChanged();
    }
}
