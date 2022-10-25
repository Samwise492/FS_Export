using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; set; }
    public AudioSource playerWalk;
    public AudioSource fireballStrike;
    public AudioSource ghostEnemyHit;
    public AudioSource collectableItem;
    public AudioSource resurrection;
    public AudioSource win;
    public AudioSource death;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Player.Instance != null)
            StartCoroutine(CheckForDeath());
    }

    IEnumerator CheckForDeath()
    {
        while (true)
        {
            if (Player.Instance.isDeath)
            {
                StartCoroutine(PlayerDeathSound());
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator PlayerDeathSound()
    {
        MusicController.Instance.background.volume = 0.1f;
        death.Play();
        yield return new WaitForSeconds(2);
        MusicController.Instance.background.volume = 0.6f;

        yield break;
    }
}
