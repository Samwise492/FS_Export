using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float bonusForce;
    private float bonusHealth;
    private float bonusDamage;
    private const float DefaultJumpForce = 7;
    private Vector3 direction;

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

        // Immortality
        if (isCheatMode) 
            Health.CurrentHealth = 1000;
        else
            return;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && groundDetection.IsGrounded)
            Jump();
#endif
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.CompareTag("Piece of light"))
        {
            PlayerInventory.Instance.Light_piecesCount++;
            PlayerInventory.Instance.light_piecesText.text = PlayerInventory.Instance.Light_piecesCount.ToString();
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Death"))
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            var pushVector = (transform.position.x - col.gameObject.transform.position.x) > 0 ? Vector3.right : Vector3.left;
            rb.AddForce(pushVector * pushForce, ForceMode2D.Impulse);
            rb.AddForce(Vector3.up * pushForce, ForceMode2D.Impulse);
            StartCoroutine(Pushing()); // player isn't able to move
        }
    }

    private void Start()
    {
        shellPool = new List<Shell>();
        for (int i = 0; i < shellsCount; i++)
        {
            var shellTemp = Instantiate(shell, shellSpawnPoint); // create the shell
            shellPool.Add(shellTemp);
            shellTemp.gameObject.SetActive(false); // turn it off in order to this object wouldn't appear when it's not necessary
        }

        buffReciever.OnBuffsChanged += BuffHandler;
    }

    public void InitUIController(UICharacterController uiController)
    {
        controller = uiController;
        controller.JumpButton.onClick.AddListener(Jump); // add method without brackets because we give link to this method, but don't invoke it 
        controller.FireButton.onClick.AddListener(CheckShoot);
    }

    public void Death()
    {
        if (playerCamera != null)
        {
            playerCamera.gameObject.transform.parent = null; // throw the object out of player on scene
            playerCamera.enabled = true; // turn on the camera   
        }
        if (deathText != null)
            deathText.gameObject.SetActive(true);
        gameObject.SetActive(false);
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

    private void BuffHandler()
    {
        var forceBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Force); // find variable which type == Force. If there is nothing it'll equal null
        var damageBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Damage);
        var healthBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Health);
        bonusForce = forceBuff == null ? 0 : forceBuff.bonus;
        bonusDamage = damageBuff == null ? 0 : damageBuff.bonus;
        bonusHealth = healthBuff == null ? 0 : healthBuff.bonus;

        health.SetHealth(bonusHealth);
        if (bonusForce != 0)
        {
            jumpForce += bonusForce;
            StartCoroutine(ForceBoost());
        }
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
        if (isReadyForShoot)
            {
                if (!isJumping && groundDetection.IsGrounded)
                {
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
        prefab.triggerDamage.Damage = TriggerDamage.DefaultDamage;
        yield break;
    }

    public IEnumerator ForceBoost()
    {
        yield return new WaitForSeconds(5);
        jumpForce = DefaultJumpForce;
        yield break;
    }
}