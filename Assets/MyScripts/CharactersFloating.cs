using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}��B��(���D)
/// </summary>
public class CharactersFloating
{
    public Transform target;//�B�Ū��� 
    public float force;//�V�W�O�q
    public float gravity;//���O

    /// <summary>
    /// �B��/���D
    /// </summary>
    /// <param name="target">�B�Ū���</param>
    /// <param name="force">�V�W�O�q</param>
    /// <param name="gravity">���O</param>
    public void OnFloating()
    {
        force -= gravity * Time.deltaTime;//�V�W�O�q
        if (force <= 0) force = 0;

        target.position = target.position + Vector3.up * force * Time.deltaTime;//����V�W
    }
}
