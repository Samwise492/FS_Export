using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [SerializeField] private FallingObject fallingLog;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject leftFist; // first fist to attack
    [SerializeField] private GameObject rightFist; // second fist to attack

    [SerializeField] private GameObject laser;
    [SerializeField] private float laserSpeed;
    [SerializeField] private GameObject laserLeftPoint;
    [SerializeField] private GameObject laserRightPoint;
    [SerializeField] private GameObject newRightPosition;
    private bool isOnRightSide;
    private bool isOnLeftSide;
    private bool isLaserRecharge;

    [SerializeField] private Tilemap temporaryTilemap;
    [SerializeField] private int tilemapLifetime;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private Health health;
    private bool isTilemapFading;
    private bool isIncreasing;
    [HideInInspector] public bool isDead;
    private float fadeValue = 1;
    private Color standartColor;

    
    [SerializeField] private Text deathText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] public AudioSource fistBumpSound;
    [SerializeField] public AudioSource laserStrikeSound;

    private void Awake()
    {
        Instance = this;  
    }

    void Start()
    {
        standartColor = temporaryTilemap.color;
        if (fallingLog.IsFallen)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(StartAttack());
        StartCoroutine(CheckHealth());
        StartCoroutine(OnLaserAttack());
    }

    private void SetAnimationFalse(string animationNames) // write variable of finished animation at first
    {
        string[] splitted = animationNames.Split(' ');
        Debug.Log(splitted[0] + ' ' + splitted[1]);
        animator.SetBool(splitted[0], false);
        StartCoroutine(DelayBeforeWave(splitted[1]));
    }
    private void LeftAttack()
    {
        StartCoroutine(FistDelay(leftFist));
    }

    private void RightAttack()
    {
        StartCoroutine(FistDelay(rightFist));
    }

    private void Update()
    {
        if (isDead == false)
        {
            if (isOnRightSide)
            {
                laser.gameObject.SetActive(true);
                if (laser.transform.position.x <= laserRightPoint.transform.position.x)
                {
                    laser.transform.Translate(Vector2.right * laserSpeed);
                }
                if (laser.transform.position.x >= laserRightPoint.transform.position.x)
                {
                    isOnRightSide = false;
                    isLaserRecharge = true;
                    laser.gameObject.transform.position = newRightPosition.gameObject.transform.position;
                }
            }

            if (isLaserRecharge)
                StartCoroutine(Recharging(3));

            if (isOnLeftSide)
            {
                if (laser.transform.position.x >= laserLeftPoint.transform.position.x)
                {
                    laser.transform.Translate(Vector2.left * laserSpeed);
                }
                if (laser.transform.position.x <= laserLeftPoint.transform.position.x)
                {
                    isOnLeftSide = false;
                    laser.gameObject.SetActive(false);
                    StartCoroutine(EndOfLaser());
                }
            }

            // Tilemap fading
            if (isTilemapFading)
            {
                if (!isIncreasing)
                {
                    fadeValue -= 0.05f;
                    temporaryTilemap.color = new Color(standartColor.r, standartColor.g, standartColor.b, fadeValue);
                    if (fadeValue <= 0.25)
                        isIncreasing = true;
                }
                if (isIncreasing)
                {
                    fadeValue += 0.05f;
                    temporaryTilemap.color = new Color(standartColor.r, standartColor.g, standartColor.b, fadeValue);
                    if (fadeValue >= 1)
                        isIncreasing = false;
                }
            }
        }
    }
    public void Death()
    {
        healthBar.SetActive(false);
        deathText.gameObject.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        isDead = true;
        StartCoroutine(DeathSound());
    }
    private void LaserBeam()
    {
        Debug.Log("Laser starts");
        Vector2 startPoint = new Vector2(laserLeftPoint.transform.position.x, laserLeftPoint.transform.position.y);
        laser.gameObject.transform.position = startPoint;

        isOnRightSide = true;
    }

    private void PlayerAttack()
    {
        StartCoroutine(TilemapLifetime());    
    }
    
    public void OnFistAttack()
    {
        fistBumpSound.Play();
    }
    
    ///////////////// coroutines /////////////////

    private IEnumerator StartAttack()
    {
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(5);
        animator.SetBool("Attack", true);
        yield break;
    }

    private IEnumerator DelayBeforeWave(string nextWave)
    {
        yield return new WaitForSeconds(5);
        animator.SetBool(nextWave, true);
        yield break;
    }

    private IEnumerator FistDelay(GameObject fist)
    {
        fist.gameObject.SetActive(true);
        fist.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 20, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        fist.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator TilemapLifetime()
    {    
        hitBox.gameObject.SetActive(true);
        temporaryTilemap.gameObject.SetActive(true);
        yield return new WaitForSeconds(tilemapLifetime - 4);

        isTilemapFading = true;       
        yield return new WaitForSeconds(4);

        isTilemapFading = false;
        hitBox.gameObject.SetActive(false);
        temporaryTilemap.gameObject.SetActive(false);
        animator.SetBool("PlayerAttack", false);
        temporaryTilemap.color = new Color(standartColor.r, standartColor.g, standartColor.b, 1);
        yield break;
    }

    private IEnumerator Recharging(int time)
    {
        laser.gameObject.SetActive(false);    
        yield return new WaitForSeconds(time);
        laser.gameObject.SetActive(true);

        isLaserRecharge = false;
        isOnLeftSide = true;
        yield break;
    }

    private IEnumerator EndOfLaser()
    {
        animator.SetBool("isLaserBeamFinished", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("isLaserBeamFinished", false);
        yield break;
    }

    private IEnumerator CheckHealth()
    {
        while (true)
        {
            if (health.CurrentHealth == 0)
                animator.SetBool("Death", true);
            if (animator.GetBool("Death") == true) // to delete
                deathText.gameObject.SetActive(true);

            if (health.CurrentHealth <= 0)
            {
                Death();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DeathSound()
    {
        MusicController.Instance.background.volume = 0.1f;
        SoundController.Instance.win.Play();
        yield return new WaitForSeconds(2);
        MusicController.Instance.background.volume = 0.6f;
        yield break;
    }

    private IEnumerator OnLaserAttack()
    {
        while (true)
        {
            if (laser.activeSelf)
                laserStrikeSound.enabled = true;
            else laserStrikeSound.enabled = false;

            yield return new WaitForEndOfFrame();
        }
    }
}
