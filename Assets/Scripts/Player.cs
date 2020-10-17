using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
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
    #region minimalHeight
    [SerializeField, HideInInspector] private float minimalHeight;
    public float MinimalHeight
    {
        get { return minimalHeight; }
        set
        {
            if (value < 5)
                minimalHeight = value;
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
    #region health
    [SerializeField] private Health health;
    public Health Health { get { return health; } }
    #endregion
    #region Singleton
    public static Player Instance { get; set; }
    #endregion
    [SerializeField] private int shellsCount;
    [SerializeField] private float shootForce = 5;
    [SerializeField] private float pushForce = 5;
    [SerializeField] private bool isCheatMode;
    private bool isReadyForShoot = true;
    private bool isJumping;
    private bool isPushing;
    private int bonusForce;
    private int bonusHealth;
    private int bonusDamage;
    private const int DefaultSpeed = 5;
    private Vector3 direction;
    private List<Shell> shellPool;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Shell shell;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private BuffReciever buffReciever;
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

        // Moving
        if (!isPushing)
        {
            direction = Vector3.zero; // (0,0)  // nullify the vector. It means if we aren't moving, then the vector won't escalate and will nullify

            if (Input.GetKey(KeyCode.A)) // GetKey means if we hold the button
            {
                direction = Vector3.left; // (-1,0) // alternative is transform.Translate
                                         
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction = Vector3.right; // (1,0) 
            }

            direction *= speed;
            direction.y = rb.velocity.y; // In order to y-axis won't nullify after new frame
        }

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

        CheckFall();

        // Immortality
        if (isCheatMode) 
            Health.CurrentHealth = 1000;
        else
            return;
    }

    private void Update()
    {
        //Shooting
        if (!isJumping && groundDetection.IsGrounded)
            CheckShoot();
        else
            return;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.CompareTag("Piece of light")) //сбор монет
        {
            PlayerInventory.Instance.Light_piecesCount++; //начисляем куски света через обращение к функции скрипта PlayerInventory
            PlayerInventory.Instance.light_piecesText.text = PlayerInventory.Instance.Light_piecesCount.ToString();
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            var pushVector = (transform.position.x - col.gameObject.transform.position.x) > 0 ? Vector3.right : Vector3.left;
            rb.AddForce(pushVector * pushForce, ForceMode2D.Impulse);
            rb.AddForce(Vector3.up * pushForce, ForceMode2D.Impulse);
            StartCoroutine(Pushing()); //player isn't able to move
        }
    }

    private void Start()
    {
        shellPool = new List<Shell>();
        for (int i = 0; i < shellsCount; i++)
        {
            var shellTemp = Instantiate(shell, shellSpawnPoint); //создаём стрелу
            //var - тип, который может стать любым другим типом. В данном случае у нас переменная shellTemp того типа, который мы получим от команд или
            //который передадим сами
            shellPool.Add(shellTemp); //добавляем стрелу в общий стак стрел
            shellTemp.gameObject.SetActive(false); //выключаем этот объект, чтобы он не появился на сцене когда не нужно
        }

        //buffReciever.onBuffsChanged += BuffHandler; // add method to reciever
        //buffReciever.onBuffsChanged();
        buffReciever.OnBuffsChanged += BuffHandler;
    }

    public void InitUIController(UICharacterController uiController)
    {
        controller = uiController;
        controller.Jump.onClick.AddListener(Jump); // add method without brackets because we give link to this method, but don't invoke it
    }

    private void CheckFall()
    {
        if (transform.position.y < minimalHeight && isCheatMode) //мы берем из элемента transform в инспекторе свойство position, а из него берем значение y.
                                                                 //если персонаж достиг нашей минимальной высоты (minimal Height), то
        {
            rb.velocity = new Vector2(0, 0); //velocity - вектор скорости. //Vector2 и Vector3 - взаимозаменяемые, то есть в двумерном пространстве нет z как 
                                             //такого, можешь использовать и то, и то. А вот в 3-хмерном пространстве это уже существенная разница

            transform.position = new Vector3(0, 0, 0); //перемещаемся в центр игрового мира
        }
        else if (transform.position.y < minimalHeight && !isCheatMode)
        {
            Destroy(gameObject); //уничтожает объект со сцены, которые мы передаем ему внутрь скобок (через инспектор)
        }
    }
    private void Jump()
    {
        if (!isPushing)
        {
            if (groundDetection.IsGrounded) //Если кнопка нажата 1 раз и мы на земле
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //прикладываем силу к объекту
                                                                          //ForceMode2D - тип силы, которую мы применяем к объекту, а Impulse - значит, что тип воздействия импульс,
                                                                          //ведь мы просто кидаем объект, а потом отпускаем его, мы не действуем на него постоянно, поэтому - импульс.
                animator.SetTrigger("StartJump"); //если мы прыгнули - активируем триггер старт джамп, означающий, что мы прыгнули
                isJumping = true; //оттолкнулись от земли - стала true
            }
        }
    }

    private void BuffHandler()
    {
        var forceBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Force); //find variable which type == Force. If there is nothing it'll equal null
        var damageBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Damage);
        var healthBuff = buffReciever.Buffs.Find(t => t.type == BuffType.Health);
        bonusForce = forceBuff == null ? 0 : forceBuff.bonus;
        bonusDamage = damageBuff == null ? 0 : damageBuff.bonus;
        bonusHealth = healthBuff == null ? 0 : healthBuff.bonus;

        health.SetHealth(bonusHealth);
        if (bonusForce != 0)
        {
            speed *= bonusForce;
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
            if (Input.GetMouseButtonDown(0)) //0 - LMB, 1 - RMB
            {
                animator.SetTrigger("isShooting");        
                Shell prefab = GetShellFromPool();

                if (bonusDamage != 0 && (prefab.triggerDamage.Damage <= TriggerDamage.DefaultDamage * 2))
                {
                    prefab.triggerDamage.Damage *= bonusDamage;
                    StartCoroutine(DamageBoost(prefab));
                }

                // vector direction (where should he flies) and fly force
                //(jump force * 20) + if flipX = true, shoot in the left side, otherwise in the right one
                prefab.SetImpulse
                    (Vector2.right, spriteRenderer.flipX ? -jumpForce * shootForce : jumpForce * shootForce, this); 
                
                StartCoroutine(Reload());
            }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.1f); // how long can player shoot
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
        speed = DefaultSpeed;
        yield break;
    }
}