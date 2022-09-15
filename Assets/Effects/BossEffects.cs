using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public GameObject ragonTongue02;
    public GameObject fireBallPos;
    public GameObject fireBreathPos;
    public GameObject boomPos;

    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    ParticleSystem a01;
    ParticleSystem a02;
    ParticleSystem a03;
    ParticleSystem a04;
    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //��o����ʧ@�ե�         
        a01 = fireBallPos.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a02 = fireBallPos.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a03 = fireBallPos.transform.GetChild(2).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        a04 = fireBallPos.transform.GetChild(3).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
    }


    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        FireBall();
        FireBreath();
        Boom();
    }

    void FireBall()
    {
        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime <= 0.5
                                     && !a01.isPlaying) a01.Play();

        if (animInfo.IsName("Attack.Attack1")
                                      && animInfo.normalizedTime <= 0.3
                                     && !a02.isPlaying) a02.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.6
                                    && animInfo.normalizedTime <= 0.65
                                   && !a03.isPlaying) a03.Play();

        if (animInfo.IsName("Attack.Attack1") && animInfo.normalizedTime > 0.45    //���y
                                      && animInfo.normalizedTime <= 0.5
                                     && !a04.isPlaying)
        {                                                                         //8�O�¸}��
            a04.transform.forward = gameObject.GetComponent<BossAI>().GetTarget().GetChild(8).position - ragonTongue02.transform.position;
            a04.Play();
        }

        if (animInfo.IsName("Attack.Attack1"))
        {
            gameObject.GetComponent<BossAI>().OnRotateToTarget();
        }
    }
    void FireBreath()
    {
      
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25    //�s��
                                     && animInfo.normalizedTime <= 0.3
                                    && !fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {                                                                                                                                        //0�O���Y
            fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().transform.forward = gameObject.GetComponent<BossAI>().GetTarget().GetChild(0).position - ragonTongue02.transform.position;
            fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        }
        if (fireBreathPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.GetComponent<BossAI>().OnRotateToTarget();
        }
    }

    void Boom()
    {
        if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime > 0.25    //��Z���z��
                                    // && animInfo.normalizedTime <= 0.45
                                    && !boomPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {                                                                                                                                        //0�O���Y
          
            boomPos.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        //if (animInfo.IsName("Attack.Attack3") && animInfo.normalizedTime > 0.6    //��Z���z��
        //                            && animInfo.normalizedTime <= 0.65
        //                           && !boomPos.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        //{       
        //    boomPos.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        //}
    }
}


