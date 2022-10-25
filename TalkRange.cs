using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkRange : MonoBehaviour
{
    public bool isAllowedToTalk;
    public static TalkRange Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        isAllowedToTalk = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isAllowedToTalk = false;
    }
}
