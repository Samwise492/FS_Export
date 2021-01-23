using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyShell : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private float lifeTime;
    [SerializeField] public TriggerDamage triggerDamage;
    private RangeEnemyShooting rangeEnemy;

    public void FireUp(Vector3 destination, float force, RangeEnemyShooting rangeEnemy)
    {
        this.rangeEnemy = rangeEnemy;
        triggerDamage.Init(this);
        triggerDamage.Parent = rangeEnemy.gameObject; // set what is parent element
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * force);

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

    }
}
