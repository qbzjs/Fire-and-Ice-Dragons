using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �u�`�h�L����
/// </summary>
public class SkeletonSoldierControl : MonoBehaviour
{
    Animator animator;
    
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer

        animator = GetComponent<Animator>();

        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
    }
}
