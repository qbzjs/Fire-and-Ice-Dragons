using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEffects : MonoBehaviour
{
    public GameObject effects;     //�w��S�Ħ�m(�]�����Q��GameObject.Find)     
    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_3;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //��o����ʧ@�ե�       
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_3 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
    }

    void Update()
    {
        // effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);   //���N�~
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //�`�ټo��
        WarNormalAttack1();
        WarNormalAttack3();
        WarSkillAttack3();
    }

    void WarNormalAttack1()
    {
        float delay = 0.35f;       //�����ɶ��I�A���O�ȥ��O����0   
        if (animInfo.IsName("Attack.NormalAttack_1") && animInfo.normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying) NormalAttack_1.Play();
            if (animInfo.normalizedTime > delay + 0.1f) NormalAttack_1.Stop();
        }
        else NormalAttack_1.Stop();
    }

    void WarNormalAttack3()
    {
        float delay = 0.55f;       //�����ɶ��I�A���O�ȥ��O����0
        if (animInfo.IsName("Attack.NormalAttack_3") && animInfo.normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying) NormalAttack_3.Play();
            if (animInfo.normalizedTime > delay + 0.1f) NormalAttack_3.Stop();
        }
        else NormalAttack_3.Stop();
    }

    void WarSkillAttack3()
    {
        var SkillAttack_30 = SkillAttack_3.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.1f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay)
        {
            if (!SkillAttack_30.isPlaying) SkillAttack_30.Play();
            if (animInfo.normalizedTime > delay + 0.1f) SkillAttack_30.Stop();
        }
        else SkillAttack_30.Stop();

        var SkillAttack_31 = SkillAttack_3.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.4f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay1)
        {
            if (!SkillAttack_31.isPlaying) SkillAttack_31.Play();
            if (animInfo.normalizedTime > delay1 + 0.1f) SkillAttack_31.Stop();
        }
        else SkillAttack_31.Stop();

        var SkillAttack_32 = SkillAttack_3.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.7f;
        if (animInfo.IsName("Attack.SkillAttack_3") && animInfo.normalizedTime > delay2)
        {
            Debug.Log("11");
            if (!SkillAttack_32.isPlaying) SkillAttack_32.Play();
            if (animInfo.normalizedTime > delay2 + 0.1f) SkillAttack_32.Stop();
        }
        else SkillAttack_32.Stop();
    }
}
