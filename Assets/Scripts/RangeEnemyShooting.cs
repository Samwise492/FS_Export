using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyShooting : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private RangeEnemyShell shell;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private int shellsCount;
    [SerializeField] private int reloadTime;
    [SerializeField] private bool isReadyForShoot = true;
    [HideInInspector] public List<RangeEnemyShell> shellPool;
    #region force
    [SerializeField] private float force; //shell force
    public float Force
    {
        get { return force; }
        set { force = value; }
    }
    #endregion force 
    void Start()
    {
        shellPool = new List<RangeEnemyShell>();
        for (int i = 0; i < shellsCount; i++)
        {
            var shellTemp = Instantiate(shell, shellSpawnPoint); // create the shell
            shellPool.Add(shellTemp);
            shellTemp.gameObject.SetActive(false); // turn it off in order to this object wouldn't appear when it's not necessary
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isShooting", true); 
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (isReadyForShoot)
            {
                RangeEnemyShell prefab = GetShellFromPool();
                prefab.FireUp(player.transform, force, this);

                StartCoroutine(Reload());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            animator.SetBool("isShooting", false);
    }

    public RangeEnemyShell GetShellFromPool()
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
    public void ReturnShellToPool(RangeEnemyShell shellTemp)
    {
        if (shellSpawnPoint != null)
        {
            if (!shellPool.Contains(shellTemp)) // if shellTemp doesn't exist in shellPool
                shellPool.Add(shellTemp);
            shellTemp.transform.parent = shellSpawnPoint; // attach shell to spawnpoint (in hierarchy)
            shellTemp.transform.position = shellSpawnPoint.transform.position; // attach shell to spawnpoint by coordinates
            shellTemp.gameObject.SetActive(false); // make shell inactive
        }
        
    }

    public IEnumerator Reload()
    {
        yield return new WaitForFixedUpdate();
        isReadyForShoot = false;
        yield return new WaitForSeconds(reloadTime); // reload time
        isReadyForShoot = true;
        yield break;
    }
}
