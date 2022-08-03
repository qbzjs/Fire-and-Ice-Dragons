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

    //�ͩR��
    LifeBar_Characters lifeBar;//�ͩR��

    //�ƭ�
    [SerializeField]float Hp;//�ͩR��
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

        //�ƭ�
        switch(gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "EnemySoldier_1":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
        }
        
        //OnSetLifeBar_Character(transform);//�]�w�ͩR��
        OnInitial();//��l��
    }

    void Update()
    {
        if(lifeBar != null) lifeBar.gameObject.SetActive(gameObject.activeSelf);
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
    /// ����������
    /// </summary>
    /// <param name="body">���è��骫��</param>
    /// <param name="active">�O�_���(1:��� 0:�����)</param>
    public void OnBodySetActive(int active)
    {
        Transform body = ExtensionMethods.FindAnyChild<Transform>(transform, "Mesh");//�k�v����
        if (body != null)
        {
            bool act = active == 1 ? act = true : act = false;
            body.gameObject.SetActive(act);
        }
    }

    /// <summary>
    /// ����v��
    /// </summary>
    /// <param name="attacker">�v���̪̪���</param>
    /// <param name="layer">�v����layer</param>
    /// <param name="heal">�^�_�q(%)</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnGetHeal(GameObject attacker, string layer, float heal, bool isCritical)
    {
        //�P�_������H
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Player" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Enemy")
        {
            Hp += MaxHp * (heal / 100);//�^�_�ͩR��
            if (Hp >= MaxHp) Hp = MaxHp;

            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

            //���ͤ�r            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//�v���ؼ�
                                 damage: MaxHp * (heal / 100),//����v��
                                 color: isCritical ? Color.yellow : Color.green,//��r�C��
                                 isCritical: isCritical);//�O�_�z��

            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);
        }
    }

    /// <summary>
    /// �s�u�L�H����v��
    /// </summary>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnConnectOtherGetHeal(float heal, bool isCritical)
    {
        Hp += MaxHp * (heal / 100);//�^�_�ͩR��
        if (Hp >= MaxHp) Hp = MaxHp;
 
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)

        if (gameObject.layer == LayerMask.NameToLayer("Player") && photonView.IsMine) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

        //���ͤ�r            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//�v���ؼ�
                             damage: MaxHp * (heal / 100),//����v��
                             color: isCritical ? Color.yellow : Color.green,//��r�C��
                             isCritical: isCritical);//�O�_�z��
     
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

            if(lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

            //���V������          
            Vector3 attackerPosition = attacker.transform.position;//�����̦V�q
            attackerPosition.y = 0;
            Vector3 targetPosition = transform.position;//�����̦V�q
            targetPosition.y = 0;
            transform.forward = attackerPosition - targetPosition;

            //���ͤ�r            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//���˥ؼ�
                                 damage: damage,//����ˮ`
                                 color: isCritical ? Color.yellow : Color.red,//��r�C��
                                 isCritical: isCritical);//�O�_�z��

            //�R���S��
            if (gameObject.layer == LayerMask.NameToLayer( "Enemy") && attacker.GetComponent<Effects>().effects.transform.GetChild(0).name.Equals("1_Warrior-NA_1"))
            {
                attacker.GetComponent<Effects>().HitEffect(attacker, gameObject.GetComponent<Collider>());
            }

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

            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHit(photonView.ViewID, transform.position, transform.rotation, damage, isCritical);

            //���`
            if (Hp <= 0)
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
    /// �s�u�L�H�������
    /// </summary>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnConnectOtherGetHit(Vector3 position, Quaternion rotation, float damage, bool isCritical)
    {
        transform.position = position;
        transform.rotation = rotation;

        Hp -= damage;//�ͩR�ȴ��
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)

        //���ͤ�r
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//���˥ؼ�
                             damage: damage,//����ˮ`
                             color: isCritical ? Color.yellow : Color.red,//��r�C��
                             isCritical: isCritical);//�O�_�z��
        
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
        //�I����
        if (GetComponent<BoxCollider>() != null)
        {
            boxCenter = GetComponent<BoxCollider>().center;
            boxSize = GetComponent<BoxCollider>().size;            
        }
        float boxCollisionDistance = boxSize.x > boxSize.z ? boxSize.x : boxSize.z;

        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        //����I��
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;
        for (int i = 0; i < rayDiration.Length; i++)
        {          
            if(Physics.Raycast(transform.position + boxCenter, rayDiration[i], out hit, boxCollisionDistance, mask))
            {
                transform.position = transform.position - rayDiration[i] * (boxCollisionDistance - 0.01f - hit.distance);
            }
        }       

        //�a�O�I��
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 2 + 0.05f, boxSize.y / 2, boxSize.z / 2 + 0.05f), transform.rotation, mask))
        {      
            if(Physics.BoxCast(transform.position + Vector3.up * boxSize.y, new Vector3(boxSize.x / 2, 0.1f, boxSize.z / 2), -transform.up, out hit, transform.rotation, boxSize.y, mask))
            {                
                if(hit.distance < boxSize.y) transform.position = transform.position + Vector3.up * (boxSize.y - 0.1f - hit.distance);
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