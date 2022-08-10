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
    ParticleSystem SkillAttack_1;
    ParticleSystem SkillAttack_3;
    ParticleSystem hit;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //��o����ʧ@�ե�        
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        NormalAttack_3 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_1 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_3 = effects.transform.GetChild(3).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        hit = effects.transform.GetChild(4).GetComponent<ParticleSystem>();              //�R���ĪG
        StarShakeSet();                                                                 //�e���_��
    }

    void Update()
    {
        // effects.transform.localPosition = new Vector3(0.2075253f, 0.8239655f, 0.4717751f);   //���N�~
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //�`�ټo��
        WarNormalAttack1();
        WarNormalAttack3();
        WarSkillAttack1();
        WarSkillAttack3();
        UpdaSnake();                                                                       //�e���_�� 
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
        float delay = 0.35f;                            //�����ɶ��I�A���O�ȥ��O����0   
        var effect = NormalAttack_3;                    //�S�ĦW��
        DoEffects(idelName, delay, effect);
    }

    void WarSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";         //�ʧ@�W��
        float delay = 0.5f;                            //�����ɶ��I�A���O�ȥ��O����0   
        var effect = SkillAttack_1;                    //�S�ĦW��

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            isshakeCamera = true;          //�e���_��
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //�ʧ@�W��
        var skill = SkillAttack_3;                     //�T�Ӥ��P�ɶ�����S��

        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.001f;                            //SkillAttack_30�S�ļ���ɶ��I�A���O�ȥ��O����0        
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.2f;                            //SkillAttack_31�S�ļ���ɶ��I
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.4f;                             //SkillAttack_32�S�ļ���ɶ��I�A���O�ȥ��O����0
        DoEffects(idelName, delay2, SkillAttack_32);

        var SkillAttack_33 = skill.transform.GetChild(3).GetComponent<ParticleSystem>();
        float delay3 = 0.7f;                             //SkillAttack_32�S�ļ���ɶ��I�A���O�ȥ��O����0
        DoEffects(idelName, delay3, SkillAttack_33);
    }

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }



    //�e���Y��
    float xTime = 0f;                                      //�Y�p���ѼơA�D�T�w�Ѽ�
    void PowerWindownView()
    {
        float yTime = 0.7f;                                //�Y�p���t�v�A�T�w�Ѽ�
        if (Input.GetKey(KeyCode.P))
        {
            isshakeCamera = true;          //�e���_��
            var star = new Vector3(Screen.width / 2, Screen.height / 2);
            star = Camera.main.ScreenToWorldPoint(star);
            var n = gameObject.transform.GetChild(3).position - star;
            xTime -= Time.deltaTime;
            Camera.main.transform.forward = n;
            Camera.main.transform.position = Camera.main.transform.position - n.normalized * xTime * yTime;
        }
        if (Input.GetKeyUp(KeyCode.P)) xTime = 0f;
    }





    #region �R���ĪG

    public void HitEffect(GameObject player, Collider hitPos)
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
        isshakeCamera = true;          //�e���_��
    }
    List<ParticleSystem> hitList = new List<ParticleSystem>();
    ParticleSystem HitPool()
    {       
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
    #endregion

    #region �R���_��   
    private float shakeTime = 0.0f;
    private float fps = 20.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    // public Camera cam;  �������Y�A��֨̿�
    public bool isshakeCamera = false;

    void StarShakeSet()
    {
        shakeTime = 0.2f;
        fps = 20.0f;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
    }
    void UpdaSnake()
    {
        if (isshakeCamera)
        {
            if (shakeTime > 0)  //���b
            {
                shakeTime -= Time.deltaTime;
                if (shakeTime <= 0)
                {
                    Camera.main.rect = new Rect(0.0f, 0.0f, 10.0f, 10.0f);
                    isshakeCamera = false;
                    shakeTime = 0.2f;
                    fps = 20.0f;
                    frameTime = 0.03f;
                    shakeDelta = 0.005f;
                }
                else
                {
                    frameTime += Time.deltaTime;

                    if (frameTime > 1.0 / fps)
                    {
                        frameTime = 0;
                        Camera.main.rect = new Rect(shakeDelta * (-5.0f + 5.0f * Random.value), shakeDelta * (-5.0f + 5.0f * Random.value), 1.0f, 1.0f);
                    }
                }
            }
        }
    }
    #endregion
}
