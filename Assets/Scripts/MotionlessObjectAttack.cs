using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionlessObjectAttack : MonoBehaviour
{
    [SerializeField] private CollisionDamage collisionDamage;
    [SerializeField] private Animator animator;

    void Update()
    {
        animator.SetBool("isCollision", collisionDamage.IsCollision);
    }
}
