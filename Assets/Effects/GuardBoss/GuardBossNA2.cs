using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBossNA2 : MonoBehaviour
{
    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    void Start()
    {
        anim = gameObject.transform.GetComponentInParent<Animator>();
        gameObject.GetComponent<ParticleSystem>().Stop();      
    }

    // Update is called once per frame
    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.25 && animInfo.normalizedTime < 0.65 && !gameObject.GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime >= 0.65)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
        }

       // TestMode();  //���ռҦ��A�Φb�s��S��
    }

    void TestMode()
    {
        gameObject.transform.GetComponentInParent<AI>().enabled = false;
      //  anim.Play("Attack.Attack2");
    }
}
