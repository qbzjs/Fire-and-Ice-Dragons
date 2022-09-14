using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    AudioSource audioSource;

    [Header("����")]
    [SerializeField] AudioClip[] thisAudioClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ���񭵮�
    /// </summary>
    /// <param name="number">���Ľs��</param>
    void OnPlayClip(int number)
    {
        if (number >= 0 && number < thisAudioClips.Length)
        {
            audioSource.clip = thisAudioClips[number];
            audioSource.Play();
        }
        else Debug.LogError("���~���Ľs��:" + number);
    }
}
