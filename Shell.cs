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
    private static int defaultShellCount;
    private int j = 0;

    private void Start()
    {
        defaultShellCount = player.shellPool.Count;
    }
    public void SetImpulse(Vector2 direction, float force, Player player)
    {
        this.player = player;
        triggerDamage.Init(this);
        triggerDamage.Parent = player.gameObject; // set what is parent element

        rb.AddForce(direction * force, ForceMode2D.Impulse); // method for shooting

        if (force < 0) // if shell flies in the left side
            transform.rotation = Quaternion.Euler(0, 180, 0); // Quaternion allows to work with rotation of object; here we do 180 degree rotation
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        if (gameObject.activeSelf == true) // it's because we can't get back shell if it doesn't exist
        {
            StartCoroutine(StartLife()); // time of shell's existence
            if (player.shellPool.Count == 0)
                StartCoroutine(FullReload());
        }
    }

    private IEnumerator FullReload()
    {
        yield return new WaitForSeconds(1);

        var unactive_shells = Resources.FindObjectsOfTypeAll<Shell>(); // find all unactive shells

        for (int i = unactive_shells.Length; i - 2 >= 0; i--)
        {
            var unactive_shell = unactive_shells[j];
            player.ReturnShellToPool(unactive_shell);
            j++;
        }

        if (j >= defaultShellCount)
            j = 0;

        yield break;
    }

    private IEnumerator StartLife() // delaying of shell's disappearance
    {
        yield return new WaitForSeconds(lifeTime); // set time until shell is disappeared
        gameObject.SetActive(false);
        yield break;
    }

    public void Destroy(GameObject gameObject)
    {
        
    }
}