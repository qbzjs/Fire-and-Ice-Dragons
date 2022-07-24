using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}��I��
/// </summary>
public class CharactersCollision : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    GameData_NumericalValue NumericalValue;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;
    float collisiionHight;//�I���ذ���(���ӱ��)
    bool isCollisionStairs;//�O�_�I���ӱ�

    //�ͩR��
    LifeBar_Characters lifeBar;//�ͩR��

    //�ƭ�
    float Hp;//�ͩR��
    float MaxHp;//�̤j�ͩR��

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//�B��/���DList

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;


        //�ƭ�
        switch(gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "SkeletonSoldier":
                MaxHp = NumericalValue.skeletonSoldierHp;
                break;
        }

        Hp = MaxHp;
        OnSetLifeBar_Character(transform);//�]�w�ͩR��
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnCollisionControl();
        OnAnimationOver();
        OnFloation();

       // if (Input.GetKeyDown(KeyCode.K)) GameSceneUI.Instance.SetPlayerHpProportion = 50 / MaxHp;//�]�w���a�ͩR�����(���a��)
    }

    /// <summary>
    /// �]�w�ͩR��_�C���}��
    /// </summary>
    /// <param name="target">���W������</param>
    void OnSetLifeBar_Character(Transform target)
    {
        lifeBar = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.lifeBar).GetComponent<LifeBar_Characters>());
        lifeBar.SetTarget = target;
    }    

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="attacker">�����̪���</param>
    /// <param name="layer">������layer</param>
    /// <param name="damage">�y���ˮ`</param>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="knockDirection">�����ĪG(0:���h, 1:����)</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnGetHit(GameObject attacker, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {

        //�P�_������H
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Enemy" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Player")
        {
            Hp -= damage;//�ͩR�ȴ��

            lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

            //���V������
            transform.forward = -attacker.transform.forward;

            //���ͤ�r
            HitNumber hitNumber = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("hitNumberNumbering"), GameSceneManagement.Instance.loadPath.hitNumber).GetComponent<HitNumber>();                            
            hitNumber.OnSetValue(target: transform,//���˥ؼ�
                                 damage: damage,//����ˮ`
                                 color: isCritical ? Color.yellow : Color.red);//��r�C��

            //�P�_�����ĪG
            switch (knockDirection)
            {
                case 0://���h
                    transform.position = transform.position + attacker.transform.forward * repel * Time.deltaTime;//���h
                    break;
                case 1://����
                    floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//�B��List
                    break;
            }

            //����Ĳ�o�ʵe
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //���A����(�����e�@�Ӱʵe)
            if (info.IsTag("KnockBack") && animationName == "Pain") animator.SetBool("KnockBack", false);
            if (info.IsTag("Pain") && animationName == "KnockBack") animator.SetBool("Pain", false);

            animator.SetBool(animationName, true);
        }        
    }

    /// <summary>
    /// �B��
    /// </summary>
    void OnFloation()
    {
        //�B��/���D
        for (int i = 0; i < floating_List.Count; i++)
        {
            floating_List[i].OnFloating();
        }

        //�I������
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            floating_List.Clear();//�M��List
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
    void OnCollisionControl()
    {
        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        //�I������
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + boxCenter + transform.up * collisiionHight, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), rayDiration[i], out hit, transform.rotation, NumericalValue.boxCollisionDistance, mask))
            {
                if (hit.transform.name == "col_2m_step") collisiionHight += 0.1f;//�ӱ�I��(�����I�ˮذ���)               
                else if(hit.transform.name == "col_1m_step") collisiionHight += 0.8f;//�ӱ�I��(�����I�ˮذ���)  
                else collisiionHight = 0;

                transform.position = transform.position - rayDiration[i] * (NumericalValue.boxCollisionDistance - hit.distance);                
            }           
        }

        //����I��(��2�h)
        if(Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), Quaternion.identity, mask))
        {
            transform.position = transform.position + transform.forward * 5 * Time.deltaTime;
        }

        //�a�O�I��
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            if (Physics.Raycast(transform.position + boxCenter, -transform.up, out hit, boxSize.y / 2, mask))//�a�O�I��(��2�h)
            {
                transform.position = transform.position + transform.up * (boxSize.y / 2 - 0.01f - hit.distance);
            }            
        }
        else
        {
            //���O
            transform.position = transform.position - Vector3.up * NumericalValue.gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// �ʵe����
    /// </summary>
    void OnAnimationOver()
    {
        if (info.IsTag("Pain") && info.normalizedTime >= 1) animator.SetBool("Pain", false);
        if (info.IsTag("KnockBack") && info.normalizedTime >= 1) animator.SetBool("KnockBack", false);       
    }

    /// <summary>
    /// �ʵe����Ĳ�o
    /// </summary>
    /// <param name="aniamtionName">�ʵe�W��</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        yield return new WaitForSeconds(0.03f);
        animator.SetBool(aniamtionName, true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + boxCenter, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2));
    }
}