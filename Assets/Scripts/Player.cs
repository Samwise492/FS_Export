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
    //private const int DefaultJumpForce = 8;
    private const int DefaultSpeed = 5;
    private Vector3 direction;
    private List<Shell> shellPool;
    [SerializeField] private Rigidbody2D rb; //это твердое тело
    [SerializeField] private Animator animator;
    [SerializeField] private Shell shell;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private BuffReciever buffReciever;
    static bool isBuffedDamage;
    static bool isBuffedForce;

    void FixedUpdate() //void значит, что после выполнения программы результаты программы не выведется, но при это она просто выполнится
    {

        animator.SetBool("isGrounded", groundDetection.IsGrounded);  //устанавливаем переменной isGrounded типа bool значение равное спорикосновению с землей

        if (!isJumping && !groundDetection.IsGrounded) //если не в прыжке и не касаемся земли
            animator.SetTrigger("StartFall");

        isJumping = isJumping && !groundDetection.IsGrounded; //принимает значение того - находимся мы на земле или нет.

        //Moving
        if (!isPushing)
        {
            direction = Vector3.zero; //(0,0)  //обнуляем вектор. То есть если никуда мы не движемся, то и вектор не возрастает. Это происходит благодаря тому, 
                                      //что каждый кадр проверяет условия и обнуляется если что

            if (Input.GetKey(KeyCode.A)) //GetKey - получаем то, что кнопка нажата
            {
                direction = Vector3.left; //(-1,0)                          //deltaTime - время между текущим и предыдущим кадром (прошедшее между кадрами); это где-то 16 милисек
                                          //transform - компонент, который перемещает объект в Unity Editor. Translate - ты говоришь куда перемещать объект
                                          //Vector2 - вектор в двоичной системе
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction = Vector3.right; //(1,0) 
            }

            direction *= speed;
            direction.y = rb.velocity.y; //чтобы после нового кадра ось y не становилась равной 0
        }

        //Pushing
        if (!isPushing)
        {
            rb.velocity = direction; //скорость равняется направлению движения (отриц. или полож.)
        }
            
        //Sprite flipping
        if (direction.x > 0) //если скорость больше 0
            spriteRenderer.flipX = false; //отменяем эффект отражения
        if (direction.x < 0)
            spriteRenderer.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(direction.x)); //устанавливаем переменной Speed типа float значение равное скорости; Mathf.Abs возвращает модуль от числа
        animator.SetBool("isPushing", isPushing);

        CheckFall(); //is player fallen

        //Immortality
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

        //Jumping
        if (!isPushing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && groundDetection.IsGrounded) //Если кнопка нажата 1 раз и мы на земле
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //прикладываем силу к объекту
                                                                          //ForceMode2D - тип силы, которую мы применяем к объекту, а Impulse - значит, что тип воздействия импульс,
                                                                          //ведь мы просто кидаем объект, а потом отпускаем его, мы не действуем на него постоянно, поэтому - импульс.
                animator.SetTrigger("StartJump"); //если мы прыгнули - активируем триггер старт джамп, означающий, что мы прыгнули
                isJumping = true; //оттолкнулись от земли - стала true
            }
        }

        /*if (!isBuffedForce)
        {
            speed = DefaultSpeed;
        }*/
    }

    void CheckFall() //проверяем упал ли игрок
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
        
        /*if (buffReciever.recievedBuff.type == BuffType.Damage)
        {
            isBuffedDamage = true;           
        }

        if (buffReciever.recievedBuff.type == BuffType.Health)
        {
            Health.CurrentHealth += buffReciever.recievedBuff.bonus; 
        }*/

        /*if (buffReciever.recievedBuff.type == BuffType.Force)
        {
            speed *= bonusForce;
            StartCoroutine(ForceBoost());
        }

        if (isBuffedForce)
        {
            
        }*/

    }

    private Shell GetShellFromPool() //вытаскиваем снаряд из пула снарядов
    {
        if (shellPool.Count > 0) //если количество снарядов в пуле > 0
        {
            var shellTemp = shellPool[0]; //создаем снаряд
            shellPool.Remove(shellTemp); // достаем снаряд из пула
            shellTemp.gameObject.SetActive(true); //делаем активным
            shellTemp.transform.parent = null; //достаем снаряд из родителя (spawnpoint всех снарядов)
            shellTemp.transform.position = shellSpawnPoint.transform.position; //выпускаем из spawnpoint'а
            return shellTemp;
        }
        return Instantiate
                    (shell, shellSpawnPoint.position, Quaternion.identity); //если вдруг снаряд из пула не вызвался, то нас подстрахует создание его через instantiate
    }

    public void ReturnShellToPool(Shell shellTemp) //возвращаем снаряд в пул снарядов
    {
        if (!shellPool.Contains(shellTemp)) //если shellTemp нету в shellPool
            shellPool.Add(shellTemp);
        shellTemp.transform.parent = shellSpawnPoint; //прикрепляем в иерархии снаряд к spawnpoint
        shellTemp.transform.position = shellSpawnPoint.transform.position; //прикрепляем по координатам снаряд к spawnpoint
        shellTemp.gameObject.SetActive(false); //отключаем снаряд   
    }

    void CheckShoot() //выстрел
    {
        if (isReadyForShoot)
            if (Input.GetMouseButtonDown(0)) //0 - лкм, 1 - пкм
            {
                animator.SetTrigger("isShooting");
                
                Shell prefab = GetShellFromPool(); //1. что мы спауним, 2. где мы спауним, 3. какой поворот объекта (у нас - никакой)
                if (bonusDamage != 0 && (prefab.triggerDamage.Damage <= TriggerDamage.DefaultDamage * 2))
                {
                    prefab.triggerDamage.Damage *= bonusDamage;
                    StartCoroutine(DamageBoost(prefab));
                }
                /*if (isBuffedDamage)
                {
                    prefab.triggerDamage.Damage *= buffReciever.recievedBuff.bonus; // double our damage
                    StartCoroutine(DamageBoost());
                }
                else prefab.triggerDamage.Damage = TriggerDamage.DefaultDamage; // set damage value to default*/
                prefab.SetImpulse
                    (Vector2.right, spriteRenderer.flipX ? -jumpForce * shootForce : jumpForce * shootForce, this); //направление вектора куда должен лететь снаряд и сила, с которой 
                //он полетит (прыжок персонажа * 20) + делаем проверку, если flipX = true, то стреляем влево, а иначе - вправо
                
                StartCoroutine(Reload());
            }
    }

    public IEnumerator Reload() //перезарядка
    {
        yield return new WaitForSeconds(0.1f); //время сколько можно стрелять игроку
        isReadyForShoot = false;
        yield return new WaitForSeconds(reloadTime); //время перезарядки
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