using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Effects : MonoBehaviour
{
    public GameObject weapon;      //�����Z��
    public GameObject effects;     //�������⨭�W��effects�A�H�w��S�Ħ�m(�]�����Q��GameObject.Find)     
    public PostProcessProfile postProcessProfile;   //����PostProcessProfile

    Animator anim;                 //��������ʧ@�ե�
    AnimatorStateInfo animInfo;    //��o�ʧ@���A(�`�ٸ}����)   
    ParticleSystem NormalAttack_1;
    ParticleSystem NormalAttack_2;
    ParticleSystem NormalAttack_3;
    ParticleSystem SkillAttack_1;
    ParticleSystem SkillAttack_2;
    ParticleSystem SkillAttack_3;
    ParticleSystem hit;

    //�Z���o��
    Color baseColor;
    float rColor, gColor, bColor;
    float intensity;
    bool lensDistortion = false;

    //�k�v�S�Ĳ�������Transform�v�T
    Transform playerEffectstoWorld;        //���⪺������
    Transform magicNa2;               //�n�������S��
    Transform magicNa30;              //�n�������S��
    Transform magicNa31;              //�n�������S��
    Transform magicNa32;              //�n�������S��
    Transform magicNa33;              //�n�������S��
    Transform magicNa34;              //�n�������S��
    Transform magicNa35;              //�n�������S��
    Transform magicBook;

    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();                             //��o����ʧ@�ե�         
        NormalAttack_1 = effects.transform.GetChild(0).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        NormalAttack_2 = effects.transform.GetChild(1).GetComponent<ParticleSystem>();    //��o�S�Ĳե�
        NormalAttack_3 = effects.transform.GetChild(2).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_1 = effects.transform.GetChild(3).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_2 = effects.transform.GetChild(4).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        SkillAttack_3 = effects.transform.GetChild(5).GetComponent<ParticleSystem>();    //��o�S�Ĳե�;
        hit = effects.transform.GetChild(6).GetComponent<ParticleSystem>();              //�R���ĪG
        StarShakeSet();                                                                 //�e���_��

        postProcessProfile.GetSetting<LensDistortion>().intensity.value = 0f;                //�p����

        playerEffectstoWorld = gameObject.transform.parent;                                //���⪺������A���S�Ĳ�������Transform�v�T      

        if (anim.runtimeAnimatorController.name == "2_Magician")                        //�n�������S��
        {
            magicNa2 = NormalAttack_2.transform.GetChild(1);                               //�����k��
            magicNa30 = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(3);        //�{�q                     
            magicNa31 = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(3);            //�{�q                    
            magicNa32 = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(3);              //�{�q 
            magicNa33 = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(3);        //�{�q                     
            magicNa34 = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(3);            //�{�q                    
            magicNa35 = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(3);              //�{�q
            magicBook = SkillAttack_1.transform.GetChild(3);                                     //�]�k��
        }

        //�Z���o���A�Ԥh
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            baseColor = weapon.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
            intensity = 1f;
            rColor = 0.933f;
            gColor = 0.933f;
            bColor = 0.933f;
        }

        //�즲�M��
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);                                      //�`�ټo��
        UpdaSnake();                                                                       //�e���_��                                                                                            
        UpdaLensDistortion();
        if (anim.runtimeAnimatorController.name == "1_Warrior")
        {
            WarNormalAttack1();
            WarNormalAttack3();
            WarSkillAttack1();
            WarSkillAttack2();
            WarSkillAttack3();
            WeaponTrailControl();  //�Z���즲�M��
        }
        if (anim.runtimeAnimatorController.name == "2_Magician")
        {
            MagNormalAttack1();
            MagNormalAttack2();
            MagNormalAttack3();
            MagSkillAttack1();
            MagSkillAttack2();
            MagSkillAttack3();
            MagEffectsControl();   //�]�k�}
        }
        if (anim.runtimeAnimatorController.name == "3_Archer")   //���@�P�A�Y�P�w�X���D�ӳo�̽T�{
        {
            ArcSkillAttack1();
            ArcSkillAttack3();
        }
    }


    //  float oSize = 0.2f;  //��v�k�}
    float booksize = 0.01262763f;
    bool closeMagicBook = false;
    void MagEffectsControl()
    {
        //��v�k�}�A����
        //var effect = SkillAttack_1;
        //if (!animInfo.IsName("Attack.SkillAttack_1"))  //�p�G���b�ɦ媬�A
        //{
        //    oSize -= oSize * 10 * Time.deltaTime;
        //    if (oSize <= 0.2f)
        //    {
        //        effect.transform.GetChild(2).gameObject.SetActive(false);
        //        oSize = 0.2f;
        //    }
        //    effect.transform.GetChild(2).GetComponent<Projector>().orthographicSize = oSize;
        //    effect.transform.GetChild(2).gameObject.transform.Rotate(0, 0, 0.5f);
        //}

        //�Ŧ�k�}�bIdle�ɰ���        
        if (animInfo.IsName("Idle") || animInfo.IsName("Attack.NormalAttack_1"))
        {
            NormalAttack_3.Stop();
        }

        //�]�k��
        if (magicBook.gameObject.activeInHierarchy)  //���]�k�ѱҰʮ�
        {
            if (!closeMagicBook)
            {
                booksize += booksize * 15f * Time.deltaTime;   //�}�l��j                
                if (booksize >= 0.01262763f) booksize = 0.01262763f;
            }

            magicBook.localScale = new Vector3(booksize, booksize, booksize);

            magicBook.Rotate(Vector3.up, 50 * Time.deltaTime, Space.World);
            magicBook.SetParent(playerEffectstoWorld);

            magicBook.position = Vector3.Lerp(magicBook.position, gameObject.transform.position + (gameObject.transform.right * (0.5f) + gameObject.transform.forward * (0.3f) + gameObject.transform.up * 2f), Time.deltaTime);
            // �����q�A�קK���|  gameObject.transform.right�����}�⪺�k��,gameObject.transform.forward �e�� gameObject.transform.up�W��

        }
        else
        {
            magicBook.SetParent(SkillAttack_1.transform);
            magicBook.gameObject.SetActive(false);
            magicBook.GetChild(1).GetComponent<ParticleSystem>().Stop();   //�����]�k�Ѯ����������S��
        }
        if (animInfo.IsName("Pain"))
        {
            closeMagicBook = true;
        }
        if (closeMagicBook)
        {
            booksize -= booksize * 45f * Time.deltaTime;   //��e���]�k�}�Ұʮɩ�j���ۤϧ@�ΡA�ҥH�ȭn�j
            if (booksize <= 0f)
            {
                magicBook.SetParent(SkillAttack_1.transform);
                magicBook.gameObject.SetActive(false);
                magicBook.GetChild(1).GetComponent<ParticleSystem>().Stop();   //�����]�k�Ѯ����������S��
                booksize = 0.008676398f;
                closeMagicBook = false;
            }
            magicBook.localScale = new Vector3(booksize, booksize, booksize);
        }



    }

    void ArcSkillAttack1()
    {
        if (animInfo.IsName("Attack.SkillAttack_1") && animInfo.normalizedTime > 0.7 && animInfo.normalizedTime <= 0.75)
        {
            if (!SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
            {
                SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            ArcSa1().transform.SetParent(SkillAttack_1.transform);
            ArcSa1().transform.localPosition = SkillAttack_1.transform.GetChild(2).localPosition;
            ArcSa1().transform.forward = SkillAttack_1.transform.forward;
            ArcSa1().Play();
        }
    }

    List<ParticleSystem> arcSa1List = new List<ParticleSystem>();
    ParticleSystem ArcSa1Pool()
    {
        ParticleSystem hitPs = Instantiate(SkillAttack_1.transform.GetChild(3).GetComponent<ParticleSystem>());
        arcSa1List.Add(hitPs);
        return hitPs;
    }
    ParticleSystem ArcSa1()
    {
        foreach (var hl in arcSa1List)
        {
            if (!hl.isPlaying) return hl;
        }
        return ArcSa1Pool();
    }

    void ArcSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";
        var effect = SkillAttack_3;
        if (animInfo.IsName(idelName) && !effect.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying) 
                                          effect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.5 
                                      && animInfo.normalizedTime <= 0.55
                                      && !effect.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
                                          effect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
    }




    #region �k�v

    void MagNormalAttack1()
    {
        if (animInfo.IsName("Attack.NormalAttack_1") && animInfo.normalizedTime > 0.3 && animInfo.normalizedTime <= 0.35)
        {
            MagNa1().transform.SetParent(NormalAttack_1.transform);
            MagNa1().transform.localPosition = NormalAttack_1.transform.GetChild(0).localPosition;
            MagNa1().transform.forward = NormalAttack_1.transform.forward;
            MagNa1().Play();
        }
    }

    List<ParticleSystem> magNa1List = new List<ParticleSystem>();
    ParticleSystem MagNa1Pool()
    {
        ParticleSystem hitPs = Instantiate(NormalAttack_1.transform.GetChild(0).GetComponent<ParticleSystem>());
        magNa1List.Add(hitPs);
        return hitPs;
    }
    ParticleSystem MagNa1()
    {
        foreach (var hl in magNa1List)
        {
            if (!hl.isPlaying) return hl;
        }
        return MagNa1Pool();
    }





    void MagNormalAttack2()
    {
        if (animInfo.IsName("Attack.NormalAttack_2"))
        {
            NormalAttack_2.Play();
            magicNa2.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
        }
        if (magicNa2.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa2.SetParent(NormalAttack_2.transform);
            magicNa2.transform.localPosition = NormalAttack_2.transform.GetChild(0).localPosition;
            magicNa2.transform.localRotation = NormalAttack_2.transform.GetChild(0).localRotation;
            magicNa2.transform.localScale = NormalAttack_2.transform.GetChild(0).localScale;
        }
    }

    void MagNormalAttack3()
    {
        if (animInfo.IsName("Attack.NormalAttack_3") && animInfo.normalizedTime <= 0.45)
        {
            NormalAttack_3.Play();
            magicNa30.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
            magicNa31.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
            magicNa32.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
            magicNa33.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
            magicNa34.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
            magicNa35.SetParent(playerEffectstoWorld);            //�S�ļ��񤧫��������Transform�v�T
        }
        if (magicNa30.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa30.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(1));
            magicNa30.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localPosition;
            magicNa30.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localRotation;
            magicNa30.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(1).GetChild(2).localScale;
        }
        if (magicNa31.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa31.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(2));
            magicNa31.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localPosition;
            magicNa31.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localRotation;
            magicNa31.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(2).GetChild(2).localScale;
        }
        if (magicNa32.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa32.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(3));
            magicNa32.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localPosition;
            magicNa32.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localRotation;
            magicNa32.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(3).GetChild(2).localScale;
        }
        if (magicNa33.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa33.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(4));
            magicNa33.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localPosition;
            magicNa33.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localRotation;
            magicNa33.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(4).GetChild(2).localScale;
        }
        if (magicNa34.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa34.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(5));
            magicNa34.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localPosition;
            magicNa34.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localRotation;
            magicNa34.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(5).GetChild(2).localScale;
        }
        if (magicNa35.GetComponent<ParticleSystem>().isStopped)  //�p�G�S�ĨS������
        {
            //�^�쨤��h�Ũë�_�����Ѽ�
            magicNa35.SetParent(NormalAttack_3.transform.GetChild(1).GetChild(6));
            magicNa35.transform.localPosition = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localPosition;
            magicNa35.transform.localRotation = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localRotation;
            magicNa35.transform.localScale = NormalAttack_3.transform.GetChild(1).GetChild(6).GetChild(2).localScale;
        }
    }


    void MagSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";

        //�]�k��
        if (animInfo.IsName(idelName))
        {
            //if (animInfo.normalizedTime > 0.001f && !magicBook.gameObject.activeInHierarchy) //���]�k�ѨS���ҰʡA�N�Y��ܤp�A���ର0
            if (!magicBook.gameObject.activeInHierarchy)
            {
                booksize = 0.000000011f;
                magicBook.gameObject.SetActive(true);  //���}�]�k�ѡA�}���α��k�}�޲z          
            }
        }


        var SkillAttack_10 = SkillAttack_1.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f) SkillAttack_10.Play();

        var SkillAttack_11 = SkillAttack_1.transform.GetChild(1).GetComponent<ParticleSystem>();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f && !SkillAttack_11.isPlaying) SkillAttack_11.Play();


        //��v�k�}
        //if (animInfo.IsName(idelName))
        //{
        //    SkillAttack_1.transform.GetChild(2).gameObject.SetActive(true);  //�k�}     
        //    oSize += oSize * 10 * Time.deltaTime;
        //    if (oSize >= 0.8f) oSize = 0.8f;
        //    SkillAttack_1.transform.GetChild(2).GetComponent<Projector>().orthographicSize = oSize;
        //    SkillAttack_1.transform.GetChild(2).gameObject.transform.Rotate(0, 0, 0.5f);
        //    //�����b�k�}�޲zMagEffectsControl����
        //}

    }

    void MagSkillAttack2()
    {
        //�Ȼs�ơA�ѨM�s��animator�y���L�k�s��I�k
        if (animInfo.IsName("Attack.SkillAttack_2") && animInfo.normalizedTime <= 0.001) SkillAttack_2.Stop();
        if (animInfo.IsName("Attack.SkillAttack_2") && animInfo.normalizedTime > 0.05 && animInfo.normalizedTime <= 0.1) SkillAttack_2.Play();
    }


    void MagSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";
        float delay = 0.01f;
        var effect = SkillAttack_3;
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying) effect.Play();
        if (animInfo.IsName(idelName) && animInfo.normalizedTime >= 0.1 && animInfo.IsName(idelName) && animInfo.normalizedTime <= 0.15) lensDistortion = true;
    }

    #endregion

    #region �Ԥh

    void WarNormalAttack1()
    {
        var idelName = "Attack.NormalAttack_1";
        if (animInfo.IsName(idelName))
        {
            intensity += intensity * 20f * Time.deltaTime;   //�ܴ��t��
            if (intensity >= 15f) intensity = 15f;  //�G��
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);


        //var idelName = "Attack.NormalAttack_1";         //�ʧ@�W��
        //float delay = 0.35f;                            //�����ɶ��I�A���O�ȥ��O����0   
        //var effect = NormalAttack_1;                    //�S�ĦW��
        //DoEffects(idelName, delay, effect);
    }

    float wNa3ScaleY = 1.5f;
    void WarNormalAttack3()
    {
        var idelName = "Attack.NormalAttack_3";         //�ʧ@�W��      
        var effect = NormalAttack_3;                    //�S�ĦW��

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.01f
                                      && !effect.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            effect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        //if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
        //                              && animInfo.normalizedTime <= 0.4f
        //                              && !effect.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        //{
        //    effect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        //}
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
                                     && animInfo.normalizedTime <= 0.37f) effect.transform.GetChild(2).GetComponent<ParticleSystem>().Play();


        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.35f
                                     && animInfo.normalizedTime <= 0.4f
                                     && !effect.transform.GetChild(3).GetComponent<ParticleSystem>().isPlaying)
        {
            effect.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        }

        //DoEffects(idelName, 0.35f, effect.transform.GetChild(1).GetComponent<ParticleSystem>());
        //DoEffects(idelName, 0.35f, effect.transform.GetChild(2).GetComponent<ParticleSystem>());

        //���ܼC�j�p
        if (animInfo.IsName(idelName))
        {
            if (animInfo.normalizedTime < 0.6)
            {
                weapon.transform.localScale = new Vector3(1f, 1.5f, 1);
                weapon.transform.localPosition = new Vector3(0, 0.49f, 0);
            }
            if (animInfo.normalizedTime >= 0.6)
            {

                wNa3ScaleY -= wNa3ScaleY * 0.5f * Time.deltaTime;
                if (wNa3ScaleY <= 1f) wNa3ScaleY = 1f;
                weapon.transform.localScale = new Vector3(1, wNa3ScaleY, 1);
                weapon.transform.localPosition = new Vector3(0, 0.347f, 0);
            }
        }
        else
        {
            wNa3ScaleY -= wNa3ScaleY * 0.5f * Time.deltaTime;
            if (wNa3ScaleY <= 1f) wNa3ScaleY = 1f;
            weapon.transform.localScale = new Vector3(1, wNa3ScaleY, 1);
            weapon.transform.localPosition = new Vector3(0, 0.347f, 0);
        }

        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(baseColor.r, gColor, bColor); ;
            gColor += gColor * 20f * Time.deltaTime;   //�ܴ��t��
            bColor += bColor * 20f * Time.deltaTime;   //�ܴ��t��
            if (gColor >= 1f) gColor = 1f;
            if (bColor >= 1.5f) bColor = 1.5f;

            intensity += intensity * 20f * Time.deltaTime;   //�ܴ��t��
            if (intensity >= 15f) intensity = 15f;  //�G��
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                gColor -= gColor * 50f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
                bColor -= bColor * 50f * Time.deltaTime;
                if (bColor <= 0.933f) bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

    }

    void WarSkillAttack1()
    {
        var idelName = "Attack.SkillAttack_1";         //�ʧ@�W��
        float delay = 0.7f;                            //�����ɶ��I�A���O�ȥ��O����0   
        var effect = SkillAttack_1;                    //�S�ĦW��

        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(rColor, gColor, bColor); ;
            rColor += rColor * 20f * Time.deltaTime;   //�ܴ��t��
            gColor += gColor * 20f * Time.deltaTime;   //�ܴ��t��
            if (rColor >= 2f) rColor = 2f;
            if (gColor >= 1.5f) gColor = 1.5f;

            intensity += intensity * 20f * Time.deltaTime;   //�ܴ��t��
            if (intensity >= 5f) intensity = 5f;  //�G��
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                rColor -= rColor * 20f * Time.deltaTime;
                if (rColor <= 0.933f) rColor = 0.933f;
                gColor -= gColor * 20f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            isshakeCamera = true;          //�e���_��
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }

    void WarSkillAttack2()
    {
        var idelName = "Attack.SkillAttack_2";         //�ʧ@�W��
        var skill = SkillAttack_2;

        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(rColor, gColor, bColor); ;
            rColor += rColor * 20f * Time.deltaTime;   //�ܴ��t��
            gColor -= gColor * 20f * Time.deltaTime;   //�ܴ��t��
            bColor -= bColor * 20f * Time.deltaTime;   //�ܴ��t��
            if (rColor >= 10) rColor = 10;
            if (gColor <= 0) gColor = 0;
            if (bColor <= 0) bColor = 0;

            intensity += intensity * 20f * Time.deltaTime;   //�ܴ��t��
            if (intensity >= 2f) intensity = 2f;  //�G��

            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 30f * Time.deltaTime;
                if (intensity <= 2f) intensity = 2f;

                rColor -= rColor * 30f * Time.deltaTime;
                if (rColor <= 0.933f) rColor = 0.933f;
                gColor = 0.933f;
                bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);




        var SkillAttack_30 = skill.transform.GetChild(0).GetComponent<ParticleSystem>();
        float delay = 0.3f;                            //SkillAttack_30�S�ļ���ɶ��I�A���O�ȥ��O����0        
                                                       // if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !SkillAttack_30.isPlaying) SkillAttack_30.Play();
        DoEffects(idelName, delay, SkillAttack_30);

        var SkillAttack_31 = skill.transform.GetChild(1).GetComponent<ParticleSystem>();
        float delay1 = 0.3f;                            //SkillAttack_31�S�ļ���ɶ��I
        DoEffects(idelName, delay1, SkillAttack_31);

        var SkillAttack_32 = skill.transform.GetChild(2).GetComponent<ParticleSystem>();
        float delay2 = 0.6f;                             //SkillAttack_32�S�ļ���ɶ��I�A���O�ȥ��O����0
        DoEffects(idelName, delay2, SkillAttack_32);
    }

    void WarSkillAttack3()
    {
        var idelName = "Attack.SkillAttack_3";         //�ʧ@�W��
        var skill = SkillAttack_3;


        if (animInfo.IsName(idelName))
        {
            baseColor = new Color(baseColor.r, gColor, bColor); ;
            gColor += gColor * 20f * Time.deltaTime;   //�ܴ��t��
            bColor += bColor * 20f * Time.deltaTime;   //�ܴ��t��
            if (gColor >= 1f) gColor = 1f;
            if (bColor >= 2f) bColor = 2f;

            intensity += intensity * 20f * Time.deltaTime;   //�ܴ��t��
            if (intensity >= 15f) intensity = 15f;  //�G��
            if (animInfo.normalizedTime > 0.5)
            {
                intensity -= intensity * 50f * Time.deltaTime;
                if (intensity <= 1f) intensity = 1f;

                gColor -= gColor * 50f * Time.deltaTime;
                if (gColor <= 0.933f) gColor = 0.933f;
                bColor -= bColor * 50f * Time.deltaTime;
                if (bColor <= 0.933f) bColor = 0.933f;
            }
        }
        weapon.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseColor * intensity);

        //�즲�M��
        if (animInfo.IsName(idelName) && animInfo.normalizedTime <= 0.4)
        {
            weapon.GetComponent<TrailRenderer>().enabled = true;
        }
        if (animInfo.normalizedTime > 0.4)
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }


        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.2f && !skill.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }

        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.7f && !skill.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.7f && !skill.transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > 0.8f && !skill.transform.GetChild(3).GetComponent<ParticleSystem>().isPlaying)
        {
            skill.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            isshakeCamera = true;          //�e���_��
        }



    }

    void WeaponTrailControl()
    {
        if (!animInfo.IsName("Attack.SkillAttack_3"))
        {
            weapon.GetComponent<TrailRenderer>().enabled = false;
        }
    }


    #endregion

    void DoEffects(string idelName, float delay, ParticleSystem effect)
    {
        if (animInfo.IsName(idelName) && animInfo.normalizedTime > delay && !effect.isPlaying)
        {
            effect.Play();
            if (animInfo.normalizedTime > delay + 0.1f) effect.Stop();
        }
        else effect.Stop();
    }


    #region �p����
    float uldTime;
    float uldSpeed;
    void UpdaLensDistortion()
    {
        if (lensDistortion)
        {
            uldTime += 2f * Time.deltaTime;
            uldSpeed += 18f * Time.deltaTime;
            postProcessProfile.GetSetting<LensDistortion>().intensity.value = uldSpeed;
            if (uldSpeed >= 35) uldSpeed = 35;
            if (uldTime >= 2) lensDistortion = false;

        }
        if (!lensDistortion)
        {
            uldSpeed -= 1000f * Time.deltaTime;
            if (uldSpeed <= 0) uldSpeed = 0;
            uldTime = 0;
            postProcessProfile.GetSetting<LensDistortion>().intensity.value = uldSpeed;
        }
    }
    #endregion

    #region �R���ĪG

    public void HitEffect(GameObject player, Collider hitPos)
    {
        Vector3 star = player.transform.GetChild(0).position;
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
        //  isshakeCamera = true;          //�e���_��
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
    bool isshakeCamera = false;

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

                    if (frameTime > 1 / fps) //�_�ʸ`���A�V�p�V�W�c
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
