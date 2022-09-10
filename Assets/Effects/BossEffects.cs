using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public GameObject ragonTongue02;
    public GameObject firePos;
    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    ParticleSystem a01;
    ParticleSystem a02;
    ParticleSystem a03;
    ParticleSystem a04;
    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //��o����ʧ@�ե�         
        a01 = firePos.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a02 = firePos.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a03 = firePos.transform.GetChild(2).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a04 = firePos.transform.GetChild(3).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
    }


    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        Fire();
    }

    void Fire()
    {
        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime <= 0.5
                                     && !a01.isPlaying) a01.Play();

        if (animInfo.IsName("Attack.Attack1")// && animInfo.normalizedTime > 0.25
                                      && animInfo.normalizedTime <= 0.3
                                     && !a02.isPlaying) a02.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.6
                                    && animInfo.normalizedTime <= 0.65
                                   && !a03.isPlaying) a03.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.45
                                      && animInfo.normalizedTime <= 0.5
                                     && !a04.isPlaying) a04.Play();

    }
}

