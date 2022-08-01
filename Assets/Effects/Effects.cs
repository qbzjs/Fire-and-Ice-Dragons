using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
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
        var idelName = "Attack.NormalAttack_1";         //�ʧ@�W��
        float delay = 0.35f;                            //�����ɶ��I�A���O�ȥ��O����0   
        var effect = NormalAttack_1;                    //�S�ĦW��
        DoEffects(idelName, delay, effect);
    }

    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //�ʧ@�W��
        float delay = 0.55f;                            //�����ɶ��I�A���O�ȥ��O����0   
        var effect = NormalAttack_3;                    //�S�ĦW��
        DoEffects(idelName, delay, effect);
    }

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //�ʧ@�W��
        var skill = SkillAttack_3;                     //�T�Ӥ��P�ɶ�����S��

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.1f;                            //SkillAttack_30�S�ļ���ɶ��I�A���O�ȥ��O����0        
        DoEffects(idelName, delay, SkillAttack_30);

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.3f;                            //SkillAttack_31�S�ļ���ɶ��I
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.7f;                             //SkillAttack_32�S�ļ���ɶ��I�A���O�ȥ��O����0
        DoEffects(idelName, delay2, SkillAttack_32);
    }

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay)
        {
            if (!effect.isPlaying) effect.Play();
        }
        else effect.Stop();
    }

    public void HitEffect(GameObject player, Collider hitPos)
    {
        if (player.tag == "Player")
        {
            Vector3 star = player.transform.GetChild(3).position;
            Vector3 dir = hitPos.transform.GetChild(0).position - star;
            if (dir.magnitude < 2)
            {
                star = new Vector3(Screen.width / 2, Screen.height / 2);
                star = Camera.main.ScreenToWorldPoint(star);
                dir = hitPos.transform.GetChild(0).position - star;
            }
            Physics.Raycast(star, dir, out RaycastHit pos, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            GetHitPs().transform.position = pos.point;
            GetHitPs().Play();
        }
    }
    List<ParticleSystem> hitList = new List<ParticleSystem>();
    ParticleSystem HitPool()
    {
        ParticleSystem hit = effects.transform.GetChild(3).GetComponent<ParticleSystem>();
        ParticleSystem hitPs = Instantiate(hit);
        hitList.Add(hitPs);
        return hitPs;
    }
    ParticleSystem GetHitPs()
    {
        foreach (var hl in hitList)
        {
            if (!hl.isPlaying) return hl;
        }
        return HitPool();
    }
}
