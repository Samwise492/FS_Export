using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float lifeTime;
    [SerializeField] public TriggerDamage triggerDamage;
    private Player player;
    #region force
    [SerializeField] private float force;
    public float Force
    {
        get { return force; }
        set { force = value; }
    }
    #endregion force

    public void Destroy(GameObject gameObject)
    {
        player.ReturnShellToPool(this);
    }

    public void SetImpulse(Vector2 direction, float force, Player player)
    {
        this.player = player;
        triggerDamage.Init(this);
        triggerDamage.Parent = player.gameObject; //устанавливаем что является родительским элементом
        rb.AddForce(direction * force, ForceMode2D.Impulse); //метод, отвечающий за выстрел снаряда
        if (force < 0) //если снаряд летит влево
            transform.rotation = Quaternion.Euler(0, 180, 0); //Quaternion позволяет работать с поворотом объекта; поворот по Y на 180 градусов
        StartCoroutine(StartLife()); 
    }

    private IEnumerator StartLife() //задержка исчезновения стрелы
    {
        yield return new WaitForSeconds(lifeTime); //ставим время до уничтожения стрелы
        Destroy(gameObject);
        yield break;
    }
}