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

        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.35 && animInfo.normalizedTime < 0.7 && !gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime > 0.35 && animInfo.normalizedTime < 0.65 && !gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }

        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime >= 0.65)
        {
            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        }
        if (animInfo.IsName("Attack.Attack2") && animInfo.normalizedTime >= 0.7)
        {
            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
       // TestMode();  //���ռҦ��A�Φb�s��S��
    }

    void TestMode()
    {
        gameObject.transform.GetComponentInParent<AI>().enabled = false;
        gameObject.transform.GetComponentInParent<CharactersCollision>().enabled = false;
        gameObject.transform.GetComponentInParent<ConnectObject>().enabled = false;
        gameObject.transform.GetComponentInParent<GuardBoss_Exclusive>().enabled = false;

        if (!animInfo.IsName("Attack.Attack2"))
        {
            anim.Play("Attack.Attack2");

        }


        //         anim.Play("Stand", 0,0f);
        //anim.speed = 0;
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    anim.speed = 1;
        //}
    }
}
