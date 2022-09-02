using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardBossNA2 : MonoBehaviour
{
    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    bool testMode = false;
    void Start()
    {
        anim = gameObject.transform.GetComponentInParent<Animator>();
        gameObject.GetComponent<ParticleSystem>().Stop();
        if (SceneManager.GetActiveScene().name.Equals("EffectScene")) testMode = true;
    }


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

        if (testMode) TestMode(); //���ռҦ��A�Φb�s��S��       
    }

    void TestMode()
    {
        gameObject.transform.GetComponentInParent<AI>().enabled = false;
        gameObject.transform.GetComponentInParent<CharactersCollision>().enabled = false;
        gameObject.transform.GetComponentInParent<ConnectObject>().enabled = false;
        gameObject.transform.GetComponentInParent<GuardBoss_Exclusive>().enabled = false;

        //if (!animInfo.IsName("Attack.Attack2"))
        //{
        //    anim.Play("Attack.Attack2");
        //}
    }
}
