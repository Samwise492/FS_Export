using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyShell : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private float lifeTime;
    [SerializeField] public TriggerDamage triggerDamage;
    RangeEnemyShooting rangeEnemy;
    #region force
    [SerializeField] private float force;
    public float Force
    {
        get { return force; }
        set { force = value; }
    }
    #endregion force

    public void FireUp(RangeEnemyShooting rangeEnemy)
    {
        this.rangeEnemy = rangeEnemy;
        triggerDamage.Init(this);
        triggerDamage.Parent = rangeEnemy.gameObject; // set what is parent element
        Instantiate(this, triggerDamage.Parent.transform);
        gameObject.transform.Translate(rangeEnemy.playerLocation * force);
        if (force < 0) // if shell flies in the left side
            transform.rotation = Quaternion.Euler(0, 180, 0); // Quaternion allows to work with rotation of object; here we do 180 degree rotation
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
