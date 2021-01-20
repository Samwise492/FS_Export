using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyShooting : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private RangeEnemyShell shell;
    [SerializeField] private Transform shellSpawnPoint;
    public List<RangeEnemyShell> shellPool;
    public Vector3 playerLocation;
    void Start()
    {
        playerLocation = player.transform.position;
    }
    private RangeEnemyShell GetShellFromPool()
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
        if (!shellPool.Contains(shellTemp)) // if shellTemp doesn't exist in shellPool
            shellPool.Add(shellTemp);
        shellTemp.transform.parent = shellSpawnPoint; // attach shell to spawnpoint (in hierarchy)
        shellTemp.transform.position = shellSpawnPoint.transform.position; // attach shell to spawnpoint by coordinates
        shellTemp.gameObject.SetActive(false); // make shell inactive
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isShooting", true);

            RangeEnemyShell prefab = GetShellFromPool();
            prefab.FireUp(this);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            animator.SetBool("isShooting", false);
    }
}
