using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a�}�⭵��
/// </summary>
public class Audio_PlayerCharacter : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSource;

    [Header("����")]
    [SerializeField] AudioClip[] thisAudioClips;

    void Start()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].volume = 0.55f;
        }
    }

    /// <summary>
    /// ���񭵮�
    /// </summary>
    /// <param name="number">���Ľs��</param>
    void OnPlayClip(int number)
    {
        if (number >= 0 && number < thisAudioClips.Length)
        {
            if (!audioSource[0].isPlaying)
            {
                audioSource[0].clip = thisAudioClips[number];
                audioSource[0].Play();
                return;
            }
            else if (!audioSource[1].isPlaying)
            {
                audioSource[1].clip = thisAudioClips[number];
                audioSource[1].Play();
                return;
            }
            else
            {
                audioSource[2].clip = thisAudioClips[number];
                audioSource[2].Play();
                return;
            }            
        }
        else Debug.LogError("���~���Ľs��:" + number);
    }
}
