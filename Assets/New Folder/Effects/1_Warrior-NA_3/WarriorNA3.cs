using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNA3 : MonoBehaviour
{
    private Animator anim;  //��������ʧ@�ե�
    float delay = 0.55f;       //�����ɶ��I�A���O�ȥ��O����0        
    ParticleSystem NormalAttack_3;    //�S�ĦW��
    ParticleSystem NormalAttack_31;    //�S�ĦW��
    ParticleSystem NormalAttack_3ps;    //�S�ĦW��

    void Start()
    {
        var pos = GameObject.Find("1_Warrior(Clone)");   //���ե�
        gameObject.transform.SetParent(pos.transform);   //���ե�

        anim = gameObject.transform.parent.GetComponent<Animator>();   //��o����ʧ@�ե�
        NormalAttack_3 = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
        NormalAttack_31 = gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
        NormalAttack_3ps = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.NormalAttack_3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay)
        {
            if (!NormalAttack_3.isPlaying)
            {
                NormalAttack_3.Play();
                NormalAttack_31.Play();
                NormalAttack_3ps.Play();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > delay + 0.1f)
            {
                NormalAttack_3.Stop();
                NormalAttack_31.Stop();
                NormalAttack_3ps.Stop();
            }
        }
        else
        {
            NormalAttack_3.Stop();
            NormalAttack_31.Stop();
            NormalAttack_3ps.Stop();
        }
        gameObject.transform.position = gameObject.transform.parent.position;
        gameObject.transform.GetChild(0).localEulerAngles = new Vector3(86.427f, 15.791f, 393.043f);
        gameObject.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(5.105f, 178.448f, 25.118f);
        gameObject.transform.GetChild(1).localEulerAngles = new Vector3(-94.184f, 44.50301f, -44.16699f);

    }
}
