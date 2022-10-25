using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player Instance { get; set; }
    #endregion
    #region speed
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > 0)
                speed = value;
        }
    }
    #endregion
    #region jumpForce
    [SerializeField] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
        set
        {
            if (value > 0)
                jumpForce = value;
        }
    }
    #endregion
    #region reloadTime
    [SerializeField] private float reloadTime;
    public float ReloadTime
    {
        get { return reloadTime; }
        set { reloadTime = value; }
    }
    #endregion
    #region shellsCount
    private int shellsCount = 3;
    public int ShellsCount => shellsCount;
    #endregion
    [SerializeField] private float shootForce = 5;
    [SerializeField] private float pushForce = 5;
    [SerializeField] private bool isCheatMode;
    private bool isReadyForShoot = true;
    private bool isJumping;
    private bool isPushing;
    public bool isDeath;
    private int isGotShadowBomb, isGotResurrection, isGotShield;

    private float bonusForce;
    private float bonusHealth;
    private float bonusDamage;
    private float bonusShield;
    #region bonusResurrection
    private float bonusResurrection;
    public float BonusResurrection
    {
        get { return bonusResurrection; }
        set
        {
            if (value > 0)
                bonusResurrection = value;
        }
    }
    #endregion
    #region bonusShadowBomb
    private float bonusShadowBomb;
    public float BonusShadowBomb => bonusShadowBomb;
    #endregion

    private const float DefaultJumpForce = 7;
    private Vector3 direction;
    private Color standartColor;

    #region health
    [SerializeField] private Health health;
    public Health Health { get { return health; } }
    #endregion
    [HideInInspector] public List<Shell> shellPool;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Shell shell;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private BuffReciever buffReciever;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Text deathText;
    [SerializeField] private GameObject resurrectionPoint;
    private UICharacterController controller;

    private void Awake()
    {
        Instance = this;       
    }
    void FixedUpdate() // void means that program result won't appear after program is compiled; but program will be done
    {
        animator.SetBool("isGrounded", groundDetection.IsGrounded);

        if (!isJumping && !groundDetection.IsGrounded)
            animator.SetTrigger("StartFall");

        isJumping = isJumping && !groundDetection.IsGrounded;

        Move();

        // Pushing
        if (!isPushing)
        {
            rb.velocity = direction; // speed equals direction of moving (negative or positive)
        }
            
        // Sprite flipping
        if (direction.x > 0) // if speed more than 0
            spriteRenderer.flipX = false; // cancel reflection effect
        if (direction.x < 0)
            spriteRenderer.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(direction.x)); // return absolute value of our speed
        animator.SetBool("isPushing", isPushing);
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && groundDetection.IsGrounded && (Time.timeScale != 0))
            Jump();
#endif
        if (controller.JumpButton.IsPressed && (Time.timeScale != 0))
            Jump();
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.CompareTag("Piece of light"))
        {
            PlayerInventory.Instance.Light_piecesCount++;
            PlayerInventory.Instance.light_piecesText.text = PlayerInventory.Instance.Light_piecesCount.ToString();
            Destroy(col.gameObject);
            SoundController.Instance.collectableItem.Play();
        }
        if (col.gameObject.CompareTag("Death"))
        {
            health.TakeHit(100);
        }
        if (col.gameObject.CompareTag("Heal"))
            SoundController.Instance.collectableItem.Play();

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (bonusShield == 1) // if bonusShield exists
            {
                System.Random Randomiser = new System.Random();
                int randomNumber = Randomiser.Next(0, 3);
                if (randomNumber == bonusShield)
                    health.CurrentHealth += 30;
            }

            var pushVector = (transform.position.x - col.gameObject.transform.position.x) > 0 ? Vector3.right : Vector3.left;
            rb.AddForce(pushVector * pushForce, ForceMode2D.Impulse);
            rb.AddForce(Vector3.up * pushForce, ForceMode2D.Impulse);
            StartCoroutine(Pushing()); // player isn't able to move
        }
    }
    private void Start()
    {
        #region fpsHandle     
        // frame handle
        const int myTargetFrameRate = 10;

        // Start off assuming that Application.targetFrameRate is 60 and QualitySettings.vSyncCount is 0
        OnDemandRendering.renderFrameInterval = 6;

        // Some applications may allow the user to modify the quality level. So we may not be able to rely on
        // the framerate always being a specific value. For this example we want the effective framerate to be 10.
        // If it is not then check the values and adjust the frame interval accordingly to achieve the framerate that we desire.
        if (OnDemandRendering.effectiveRenderFrameRate != 10)
        {
            if (QualitySettings.vSyncCount > 0)
            {
                OnDemandRendering.renderFrameInterval = (Screen.currentResolution.refreshRate / QualitySettings.vSyncCount / myTargetFrameRate);
            }
            else
            {
                OnDemandRendering.renderFrameInterval = (Application.targetFrameRate / myTargetFrameRate);
            }
        }
        StartCoroutine(SlowRenderingFor5Seconds());
        #endregion

        shellPool = new List<Shell>();
        for (int i = 0; i < shellsCount; i++)
        {
            var shellTemp = Instantiate(shell, shellSpawnPoint); // create the shell
            shellPool.Add(shellTemp);
            shellTemp.gameObject.SetActive(false); // turn it off in order to this object wouldn't appear when it's not necessary
        }

        buffReciever.OnBuffsChanged += BuffHandler;

        standartColor = spriteRenderer.color;

        if (PlayerPrefs.HasKey("Player_Health"))
            health.CurrentHealth = PlayerPrefs.GetInt("Player_Health");
        if (PlayerPrefs.HasKey("Player_Coins"))
            PlayerInventory.Instance.Light_piecesCount = PlayerPrefs.GetInt("Player_Coins");
        if (PlayerPrefs.HasKey("ShadowBomb"))
            isGotShadowBomb = PlayerPrefs.GetInt("ShadowBomb");
        if (PlayerPrefs.HasKey("Resurrection"))
            isGotShadowBomb = PlayerPrefs.GetInt("Resurrection");
        if (PlayerPrefs.HasKey("Shield"))
            isGotShadowBomb = PlayerPrefs.GetInt("Shield");

        StartCoroutine(CheckResurrection());
        StartCoroutine(CheckForBuffs());
    }

    public void InitUIController(UICharacterController uiController)
    {
        controller = uiController;
        //controller.JumpButton.onClick.AddListener(Jump); // add method without brackets because we give link to this method, but don't invoke it 
        controller.FireButton.onClick.AddListener(CheckShoot);
        controller.ShadowBombButton.onClick.AddListener(ShadowFlame);
    }

    public void Death()
    {
        if (playerCamera != null)
        {
            playerCamera.gameObject.transform.parent = null; // throw the object out of player on scene
            playerCamera.enabled = true; // turn on the camera   
        }
        if (deathText != null)
        {
            deathText.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if (bonusResurrection == 0)
            isDeath = true;
    }

    private void Move()
    {
        if (!isPushing)
        {
            direction = Vector3.zero; // (0,0)  // nullify the vector. It means if we aren't moving, then the vector won't escalate and will nullify
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A))
            {
                direction = Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction = Vector3.right;
            }
#endif
            if (controller.LeftButton.IsPressed)
            {
                direction = Vector3.left; // (-1,0) // alternative is transform.Translate

            }
            if (controller.RightButton.IsPressed)
            {
                direction = Vector3.right; // (1,0) 
            }

            direction *= speed;
            direction.y = rb.velocity.y; // In order to y-axis won't nullify after new frame
        }
    }

    public void SavePlayerPrefs()
    {
        health.CurrentHealth = 100;
        PlayerPrefs.SetInt("Player_Coins", PlayerInventory.Instance.Light_piecesCount);
        PlayerPrefs.SetInt("ShadowBomb", isGotShadowBomb);
        PlayerPrefs.SetInt("Resurrection", isGotResurrection);
        PlayerPrefs.SetInt("Shield", isGotShield);
    }

    private void Jump()
    {
        if (!isPushing)
        {
            if (groundDetection.IsGrounded)
            {
                // apply force to the object
                // ForceMode2D - kind of force, which we apply to the obejct; Impulse means that kind of force is impulse.
                // we just throw the object, and release it. We don't do it continiously, because it is impulse
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 

                animator.SetTrigger("StartJump");
                isJumping = true;
            }
        }
    }

    private void ShadowFlame()
    {
        StartCoroutine(ShadowBomb());
    }

    private void BuffHandler()
    {
        var forceBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Force); // find variable which type == Force. If there is nothing it'll equal null
        var damageBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Damage);
        var healthBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Health);
        bonusForce = forceBuff == null ? 0 : forceBuff.bonus;
        bonusDamage = damageBuff == null ? 0 : damageBuff.bonus;
        bonusHealth = healthBuff == null ? 0 : healthBuff.bonus;

        var shieldBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Shield);
        var resurrectionBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Resurrection);
        var shadowBombBuff = buffReciever.Buffs.Find(t => t.type == BuffType.ShadowBomb);
        if (shieldBuff != null)
            bonusShield = shieldBuff.bonus;
        if (resurrectionBuff != null)
            bonusResurrection = resurrectionBuff.bonus;
        if (shadowBombBuff != null)
            bonusShadowBomb = shadowBombBuff.bonus;

        if (bonusForce != 0)
        {
            jumpForce += bonusForce;
            StartCoroutine(ForceBoost());
        }
        if (bonusHealth != 0)
        {
            health.SetHealth(bonusHealth);
        }

        if (bonusShadowBomb != 0)
        {
            isGotShadowBomb = 1;
            controller.ShadowBombButton.gameObject.SetActive(true);
            controller.ShadowBombBackground.gameObject.SetActive(true);
        }
        if (bonusResurrection != 0)
        {
            isGotResurrection = 1;
        }
        if (bonusShield != 0)
        {
            isGotShield = 1;
        }
        //if (bonusResurrection != 1 || bonusShield != 1 || bonusShadowBomb != 1)
        buffReciever.Buffs.Clear();
    }

    private Shell GetShellFromPool()
    {
        if (shellPool.Count > 0) // if number of shells > 0
        {
            var shellTemp = shellPool[0]; // create shell
            shellPool.Remove(shellTemp); // pull out shell from pool
            shellTemp.gameObject.SetActive(true); // make shell active
            shellTemp.transform.parent = null; // pull out shell from parent object (spawnpoint of all shells)
            shellTemp.transform.position = shellSpawnPoint.transform.position; // release from spawnpoint
            return shellTemp;         
        }
        
        return Instantiate
            (shell, shellSpawnPoint.position, Quaternion.identity); // if suddenly shell wasn't invoked from pool, we'll create it through Instantiate
    }
    public void ReturnShellToPool(Shell shellTemp)
    {
        if (!shellPool.Contains(shellTemp)) // if shellTemp doesn't exist in shellPool
            shellPool.Add(shellTemp);
        shellTemp.transform.parent = shellSpawnPoint; // attach shell to spawnpoint (in hierarchy)
        shellTemp.transform.position = shellSpawnPoint.transform.position; // attach shell to spawnpoint by coordinates
        shellTemp.gameObject.SetActive(false); // make shell inactive
    }
    void CheckShoot()
    {
        if (isReadyForShoot && shellPool.Count > 0 && (Time.timeScale != 0))
            {
                if (!isJumping && groundDetection.IsGrounded)
                {
                    SoundController.Instance.fireballStrike.Play();
                    animator.SetTrigger("isShooting");
                    Shell prefab = GetShellFromPool();

                    if (bonusDamage != 0 && (prefab.triggerDamage.Damage <= TriggerDamage.DefaultDamage * 2))
                    {
                        prefab.triggerDamage.Damage *= bonusDamage;
                        StartCoroutine(DamageBoost(prefab));
                    }

                    // vector direction (where should it flies) and fly force
                    //(jump force * 20) + if flipX = true, shoot in the left side, otherwise in the right one
                    prefab.SetImpulse
                        (Vector2.right, spriteRenderer.flipX ? -jumpForce * shootForce : jumpForce * shootForce, this);
                    
                    StartCoroutine(Reload());
                }
            }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.05f); // how long can player shoot
        isReadyForShoot = false;
        yield return new WaitForSeconds(reloadTime); // reload time
        isReadyForShoot = true;
        yield break;
    }

    public IEnumerator Pushing()
    {
        isPushing = true;
        yield return new WaitForSeconds(2);
        isPushing = false;
        yield break;
    }

    public IEnumerator DamageBoost(Shell prefab)
    {
        yield return new WaitForSeconds(5);
        bonusDamage = 0;
        prefab.triggerDamage.Damage = TriggerDamage.DefaultDamage;
        yield break;
    }

    public IEnumerator ForceBoost()
    {
        yield return new WaitForSeconds(5);
        jumpForce = DefaultJumpForce;
        yield break;
    }

    public IEnumerator ShadowBomb()
    {
        spriteRenderer.color = new Color(168, 0, 181, 255); //A800B5
        transform.gameObject.tag = "Untagged";
        
        yield return new WaitForSeconds(3);

        spriteRenderer.color = standartColor;
        transform.gameObject.tag = "Player";

        controller.ShadowBombButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        controller.ShadowBombButton.gameObject.SetActive(true);
        yield break;
    }

    public IEnumerator CheckResurrection()
    {
        while (true)
        {
            if (Health.CurrentHealth <= 0 && bonusResurrection == 1)
            {
                Health.CurrentHealth = 100;
                transform.position = resurrectionPoint.transform.position + new Vector3(0, 2f, 0);
                bonusResurrection = 0;
                SoundController.Instance.resurrection.Play();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CheckForBuffs()
    {
        while (true)
        {
            if (PlayerPrefs.GetInt("ShadowBomb") == 1)
            {
                bonusShadowBomb = 1;
                controller.ShadowBombButton.gameObject.SetActive(true);
                controller.ShadowBombBackground.gameObject.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Resurrection") == 1)
            {
                bonusResurrection = 1;
            }
            if (PlayerPrefs.GetInt("Shield") == 1)
            {
                bonusShield = 1;
            }

            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator SlowRenderingFor5Seconds()
    {
        // After 5 seconds go back to rendering every frame
        yield return new WaitForSeconds(5);
        OnDemandRendering.renderFrameInterval = 1;
    }

    public void WalkSound()
    {
        SoundController.Instance.playerWalk.Play();
    }

    public void FireSound()
    {
        SoundController.Instance.fireballStrike.Play();
    }
}