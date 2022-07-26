using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNA1 : MonoBehaviour
{
    private Animator anim;  //��������ʧ@�ե�
    float delay = 0.35f;       //�����ɶ��I�A���O�ȥ��O����0        
    ParticleSystem NormalAttack_1;    //�S�ĦW��
    ParticleSystem NormalAttack_1ps;    //�S�ĦW��

    void Start()
    {
        var pos = GameObject.Find("1_Warrior(Clone)");   //���ե�
        gameObject.transform.SetParent(pos.transform);   //���ե�

        anim = gameObject.transform.parent.GetComponent<Animator>();   //��o����ʧ@�ե�
        NormalAttack_1 = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
        NormalAttack_1ps = gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_1.isPlaying)
            {
                NormalAttack_1.Play();
                NormalAttack_1ps.Play();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_1.Stop();
                NormalAttack_1ps.Stop();
            }
        }
        else
        {
            NormalAttack_1.Stop();
        }
        gameObject.transform.position = gameObject.transform.parent.position;
        gameObject.transform.GetChild(0).localEulerAngles = new Vector3(125.485f, 284.455f, -5.64801f);
        gameObject.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(3.294599f, 3.294599f, -3.294599f);
    }


}
