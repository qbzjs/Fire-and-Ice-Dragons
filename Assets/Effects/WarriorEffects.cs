using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEffects : MonoBehaviour
{
    private Animator anim;  //��������ʧ@�ե�
    public GameObject effects;  //�w��S�Ħ�m(�]�����Q��GameObject.Find) 
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_3;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();   //��o����ʧ@�ե�
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
    }

    void Update()
    {
        effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);
        WarNormalAttack1();
        WarNormalAttack3();
    }

    void WarNormalAttack1()
    {
        float delay = 0.35f;       //�����ɶ��I�A���O�ȥ��O����0   
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying)
            {
                NormalAttack_1.Play();

            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_1.Stop();
            }
        }
        else
        {
            NormalAttack_1.Stop();
        }
    }

    void WarNormalAttack3()
    {
        float delay = 0.55f;       //�����ɶ��I�A���O�ȥ��O����0
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying)
            {
                NormalAttack_3.Play();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_3.Stop();
            }
        }
        else
        {
            NormalAttack_3.Stop();
        }
    }
}
