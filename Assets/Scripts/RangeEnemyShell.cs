using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyShell : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private float lifeTime;
    [SerializeField] public TriggerDamage triggerDamage;
    [SerializeField] private Rigidbody2D rb;
    private RangeEnemyShooting rangeEnemy;
    private RangeEnemyShell shell;

    public void FireUp(Transform player, float force, RangeEnemyShooting rangeEnemy)
    {
        this.rangeEnemy = rangeEnemy;
        shell = this;

        triggerDamage.Init(this);
        triggerDamage.Parent = rangeEnemy.gameObject; // set what is parent element

        Vector2 direction = (Vector2)player.position - rb.position; // where our shell will go
        direction.Normalize();

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(StartLife());
    }
    private IEnumerator StartLife() // delaying of shell's disappearance
    {
        yield return new WaitForSeconds(lifeTime); // set time until shell is disappeared
        rangeEnemy.ReturnShellToPool(this);
        yield break;
    }
    public void Destroy(GameObject gameObject)
    {
        rangeEnemy.ReturnShellToPool(shell);
    }
}
