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
    GameData_NumericalValue NumericalValue;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;
    Vector3 safePosition;//�����d��e����m

    float collisiionHight;//�I���ذ���(���ӱ��)

    //�ͩR��
    LifeBar_Characters lifeBar;//�ͩR��

    //�ƭ�
    float Hp;//�ͩR��
    float MaxHp;//�̤j�ͩR��

    public bool isDie;//�O�_���`

    public List<CharactersFloating> floating_List = new List<CharactersFloating>();//�B��/���DList

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;
        
        //�I����
        if (GetComponent<BoxCollider>() != null)
        {
            boxCenter = GetComponent<BoxCollider>().center;
            boxSize = GetComponent<BoxCollider>().size;
        }

        //�ƭ�
        switch(gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "Enemy":
                MaxHp = NumericalValue.enemyHp;
                break;
        }
        
        OnSetLifeBar_Character(transform);//�]�w�ͩR��
        OnInitial();//��l��
    }

    void Update()
    {
        lifeBar.gameObject.SetActive(gameObject.activeSelf);
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;//�s�u�Ҧ�      

        OnCollisionControl();
        OnAnimationOver();
        OnFloation();

        //���ե�
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit(gameObject, "Enemy", 100, "Pain", 0, 1, false);
    }

    /// <summary>
    /// ��l��
    /// </summary>
    public void OnInitial()
    {
        Hp = MaxHp;
        
        //�ͩR��(�Y��)
        if (lifeBar != null)
        {
            lifeBar.SetValue = Hp / MaxHp;
        }
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
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //�{��
        if (info.IsName("Dodge") || info.IsName("Die")) return;

        //�P�_������H
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Enemy" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Player")
        {
            Hp -= damage;//�ͩR�ȴ��
            if (Hp <= 0) Hp = 0;

            lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

            //���V������
            transform.forward = -attacker.transform.forward;                        

            //���ͤ�r
            HitNumber hitNumber = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("hitNumberNumbering"), GameSceneManagement.Instance.loadPath.hitNumber).GetComponent<HitNumber>();                            
            hitNumber.OnSetValue(target: transform,//���˥ؼ�
                                 damage: damage,//����ˮ`
                                 color: isCritical ? Color.yellow : Color.red,//��r�C��
                                 isCritical: isCritical);//�O�_�z��
                        

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

            //���`
            if(Hp <= 0)
            {                
                isDie = true;
                animator.SetTrigger("Die");
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Die", "Die");
                return;
            }

            //����Ĳ�o�ʵe
            if (info.IsTag(animationName))
            {
                StartCoroutine(OnAniamtionRepeatTrigger(animationName));
                return;
            }

            //���A����(�����e�@�Ӱʵe)
            if (info.IsTag("KnockBack") && animationName == "Pain")
            {
                animator.SetBool("KnockBack", false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "KnockBack", false);
            }
            if (info.IsTag("Pain") && animationName == "KnockBack")
            {
                animator.SetBool("Pain", false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Pain", false);
            }

            //�ݾ� & �b�] �~��������ʵe
            if (info.IsName("Idle") || info.IsName("Run"))
            {
                animator.SetBool(animationName, true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, true);
            }
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
            /*if (Physics.BoxCast(transform.position + boxCenter + transform.up * collisiionHight, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), rayDiration[i], out hit, transform.rotation, NumericalValue.boxCollisionDistance, mask))
            {
                if (hit.transform.name == "col_2m_step") collisiionHight += 0.3f;//�ӱ�I��(�����I�ˮذ���)   
                else if (hit.transform.name == "col_1m_step") collisiionHight += 0.8f;//�ӱ�I��(�����I�ˮذ���)                
                else collisiionHight = 0;

                transform.position = transform.position + transform.up * collisiionHight - rayDiration[i] * (NumericalValue.boxCollisionDistance - hit.distance);                
            } */

            if(Physics.Raycast(transform.position + boxCenter + transform.up * collisiionHight, rayDiration[i], out hit, NumericalValue.boxCollisionDistance, mask))
            {
                transform.position = transform.position + transform.up * collisiionHight - rayDiration[i] * (NumericalValue.boxCollisionDistance - hit.distance);
            }
        }

       /* //����I��(��2�h)
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2, boxSize.y / 4, boxSize.z / 2), Quaternion.identity, mask))
        {
            transform.position = safePosition;
        }
        else safePosition = transform.position;*/

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
            transform.position = transform.position - Vector3.up * NumericalValue.gravity * Time.deltaTime;//���O
        }
    }

    /// <summary>
    /// �ʵe����
    /// </summary>
    void OnAnimationOver()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsTag("Pain") && info.normalizedTime >= 1)
        {
            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Pain", false);
        }

        if (info.IsTag("KnockBack") && info.normalizedTime >= 1)
        {
            animator.SetBool("KnockBack", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "KnockBack", false);
        }
        if (info.IsTag("Die") && info.normalizedTime >= 1)
        {            
            //�s�u�Ҧ�
            if (GameDataManagement.Instance.isConnect && photonView.IsMine)
            {                
                PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
            }

            //��������
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �ʵe����Ĳ�o
    /// </summary>
    /// <param name="aniamtionName">�ʵe�W��</param>
    /// <returns></returns>
    IEnumerator OnAniamtionRepeatTrigger(string aniamtionName)
    {
        animator.SetBool(aniamtionName, false);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, aniamtionName, false);

        yield return new WaitForSeconds(0.03f);

        animator.SetBool(aniamtionName, true);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, aniamtionName, true);
    }       
}