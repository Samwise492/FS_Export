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
        triggerDamage.Parent = player.gameObject; // set what is parent element
        rb.AddForce(direction * force, ForceMode2D.Impulse); // method for shooting
        if (force < 0) // if shell flies in the left side
            transform.rotation = Quaternion.Euler(0, 180, 0); // Quaternion allows to work with rotation of object; here we do 180 degree rotation
        StartCoroutine(StartLife()); 
    }

    private IEnumerator StartLife() // delaying of shell's disappearance
    {
        yield return new WaitForSeconds(lifeTime); // set time until shell is disappeared
        Destroy(gameObject);
        yield break;
    }
}