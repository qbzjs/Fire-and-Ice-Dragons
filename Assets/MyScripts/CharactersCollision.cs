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
    public Vector3 boxCenter;
    public Vector3 boxSize;
    [Header("�Z���a������")] public float heightFromGround;
    [Header("�𭱸I���Z��")] public float wallCollisionDistance;
    [Header("�𭱸I������")] public float wallCollisionHight;
    [Tooltip("�I���ؤj�p_�}��")] public float collisionSize_Character;//�I���ؤj�p_�}��
    //[Tooltip("�I���ر��O_�}��")] public float collisionPushForce_Character;//�I���ر��O_�}��
    float boxCollisionDistance;//�I���Z��
    public Transform[] collisionObject = new Transform[9];//�I������(�P�w�O�_���I��)    
    public Transform[] GetCollisionObject => collisionObject;
    float jumpRayDistance;//���D�g�u�Z��    

    //�ͩR��
    LifeBar_Characters lifeBar;//�ͩR��

    //�ƭ�
    public float Hp;//�ͩR��
    public float MaxHp;//�̤j�ͩR��
    public float addDefence;//�W�[���m��
    public bool isSuckBlood;//�O�_���l��ĪG
    public bool isSelfHeal;//�O�_���^�_�ĪG
    float selfTime;//�ۧڦ^�_�ɶ�    
    public float acceleration;//�[�t��

    //�P�_
    public bool isDie;//�O�_���`
    bool isFall;//�O�_���U

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
        boxCollisionDistance = boxSize.x < boxSize.z ? boxSize.x / 2 : boxSize.z / 2;//�I���Z��
        heightFromGround = -0.063f;//�Z���a������        
        wallCollisionDistance = 0.25f;//�𭱸I���Z��
        wallCollisionHight = 0.5f;//�𭱸I������
        acceleration = 1;//�[�t��
        collisionSize_Character = 0.7f;//�I���ؤj�p_�}��
        //collisionPushForce_Character = 0.77f;//�I���ر��O_�}��

        //�}��Tag�]�wHP
        switch (gameObject.tag)
        {
            case "Player":
                MaxHp = NumericalValue.playerHp;
                break;
            case "EnemySoldier_1":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
            case "EnemySoldier_2":
                MaxHp = NumericalValue.enemySoldier1_Hp;
                break;
        }

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            //�W�[�̤j�ͩR��
            if (GameDataManagement.Instance.equipBuff[i] == 0 && GetComponent<PlayerControl>()) MaxHp += MaxHp * (GameDataManagement.Instance.numericalValue.buffAbleValue[0] / 100);
        }

        //OnSetLifeBar_Character(transform);//�]�w�ͩR��
        OnInitial();//��l��
    }

    void Update()
    {
        if (lifeBar != null) lifeBar.gameObject.SetActive(gameObject.activeSelf);

        if (!GameDataManagement.Instance.isConnect || photonView.IsMine)
        {                        
            OnSelfHeal();
            OnCollisionControl();
            OnFloation();
            OnAnimationOver();
        }        

        //���ե�
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit(gameObject,gameObject, "Enemy", 100, "Pain", 0, 1, false);
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
        Transform body = ExtensionMethods.FindAnyChild<Transform>(transform, "Mesh");//���骫��
        if (body != null)
        {
            bool act = active == 1 ? act = true : act = false;
            body.gameObject.SetActive(act);
        }
    }

    /// <summary>
    /// �ۨ��^�_
    /// </summary>
    void OnSelfHeal()
    {
        if (Hp <= 0) return;//���F

        if(isSelfHeal)
        {
            selfTime -= Time.deltaTime;
            if(selfTime <= 0 && Hp < MaxHp)
            {
                selfTime = NumericalValue.playerSelfHealTime;//���s�ɶ�

                float heal = MaxHp * (NumericalValue.buffAbleValue[5] / 100);//�^�_�ͩR��
                Hp += heal;//�^�_�ͩR��
                if (Hp >= MaxHp) Hp = MaxHp;

                //���ͤ�r            
                HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
                hitNumber.OnSetValue(target: transform,//�v���ؼ�
                                     damage: heal,//����v��
                                     color: Color.green,//��r�C��
                                     isCritical: false);//�O�_�z��

                //�s�u
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, false);

                    //�ۤv
                    if (photonView.IsMine)
                    {
                        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
                        if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
                    }
                }
                else
                {
                    if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
                    if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
                }
            }
        }
    }

    /// <summary>
    /// �l��ĪG
    /// </summary>
    /// <param name="heal">�^�_�q</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnSuckBlood(float heal, bool isCritical)
    {
        if (Hp <= 0) return;//���F

        float suckBlood = heal * (NumericalValue.buffAbleValue[4] / 100);//�l���

        Hp += suckBlood;//�^�_�ͩR��
        if (Hp >= MaxHp) Hp = MaxHp;

        //���ͤ�r            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//�v���ؼ�
                             damage: suckBlood,//����v��
                             color: isCritical ? Color.green : Color.green,//��r�C��
                             isCritical: isCritical);//�O�_�z��

        //�s�u
        if (GameDataManagement.Instance.isConnect)
        {
            PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);

            //�ۤv
            if (photonView.IsMine)
            {
                if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
                if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
            }
        }
        else
        {
            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
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

            //���ͤ�r            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//�v���ؼ�
                                 damage: MaxHp * (heal / 100),//����v��
                                 color: isCritical ? Color.green : Color.green,//��r�C��
                                 isCritical: isCritical);//�O�_�z��

            //�s�u
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendGetHeal(photonView.ViewID, heal, isCritical);

                //�ۤv
                if (photonView.IsMine)
                {
                    if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
                    if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
                }
            }
            else
            {
                if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
                if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
            }
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

        //���ͤ�r            
        HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
        hitNumber.OnSetValue(target: transform,//�v���ؼ�
                             damage: MaxHp * (heal / 100),//����v��
                             color: isCritical ? Color.green : Color.green,//��r�C��
                             isCritical: isCritical);//�O�_�z��
        Debug.LogError(Hp + ":" + transform.name);
        if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)
        if (gameObject.layer == LayerMask.NameToLayer("Player") && photonView.IsMine) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="attacker">������</param>
    /// <param name="attackerObject">�����̪���</param>
    /// <param name="layer">������layer</param>
    /// <param name="damage">�y���ˮ`</param>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="knockDirection">�����ĪG(0:���h, 1:����)</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnGetHit(GameObject attacker, GameObject attackerObject, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        //�{��
        if (info.IsName("Dodge") || info.IsName("Die")) return;
        
        //�P�_������H
        if (gameObject.layer == LayerMask.NameToLayer("Player") && layer == "Enemy" ||
            gameObject.layer == LayerMask.NameToLayer("Enemy") && layer == "Player")
        {
            float getDamge = (damage - (damage * (addDefence / 100))) < 0 ? 0: damage - addDefence;//���쪺��`

            //�P�_�����̬O�_���l��ĪG
            if (attacker.TryGetComponent(out CharactersCollision attackerCollision))
            {
                if (attackerCollision.isSuckBlood) attackerCollision.OnSuckBlood(getDamge, isCritical);               
            }

            Hp -= getDamge;//�ͩR�ȴ��
            if (Hp <= 0) Hp = 0;

            if (lifeBar != null) lifeBar.SetValue = Hp / MaxHp;//�]�w�ͩR�����(�Y��)            
            if (gameObject.layer == LayerMask.NameToLayer("Player")) GameSceneUI.Instance.SetPlayerHpProportion = Hp / MaxHp;//�]�w���a�ͩR�����(���a��)

            //���V������          
            Vector3 attackerPosition = attackerObject.transform.position;//�����̦V�q
            attackerPosition.y = 0;
            Vector3 targetPosition = transform.position;//�����̦V�q
            targetPosition.y = 0;
            transform.forward = attackerPosition - targetPosition;

            //���ͤ�r            
            HitNumber hitNumber = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.hitNumber)).GetComponent<HitNumber>();
            hitNumber.OnSetValue(target: transform,//���˥ؼ�
                                 damage: getDamge,//����ˮ`
                                 color: isCritical ? Color.yellow : Color.red,//��r�C��
                                 isCritical: isCritical);//�O�_�z��

            //�R���S��
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (attackerObject.GetComponent<Effects>()!=null )                   
                {
                     attackerObject.GetComponent<Effects>().HitEffect(attackerObject, gameObject.GetComponent<Collider>());
                }
            }

            //���O�s�u || �ХD
            if (!GameDataManagement.Instance.isConnect || photonView.IsMine)
            {
                //�P�_�����ĪG
                switch (knockDirection)
                {
                    case 0://���h
                        LayerMask mask = LayerMask.GetMask("StageObject");
                        if (!Physics.Raycast(transform.position + boxCenter, -transform.forward, 1f, mask))//�I�𤣦A���h
                        {
                            transform.position = transform.position + attackerObject.transform.forward * repel * Time.deltaTime;//���h
                        }                        
                        break;
                    case 1://����                        
                        floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//�B��List                    
                        break;
                }
            }

            //�s�u
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendGetHit(targetID: photonView.ViewID,
                                                                                           position: transform.position,
                                                                                           rotation: transform.rotation,
                                                                                           damage: getDamge,
                                                                                           isCritical: isCritical,
                                                                                           knockDirection: knockDirection,
                                                                                           repel: repel,
                                                                                           attackerObjectID: attackerObject.GetPhotonView().ViewID);

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
            if (info.IsTag("KnockBack") && animationName == "Pain"||
                info.IsTag("Pain") && animationName == "KnockBack")
            {
                animator.SetBool(animationName, false);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, false);
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
    /// <param name="knockDirection">���h��V</param>
    /// <param name="attackObj">�����̪���</param>
    public void OnConnectOtherGetHit(Vector3 position, Quaternion rotation, float damage, bool isCritical, int knockDirection, float repel, GameObject attackObj)
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

        //�P�_�����ĪG
        switch (knockDirection)
        {
            case 0://���h
                LayerMask mask = LayerMask.GetMask("StageObject");
                if (!Physics.Raycast(transform.position + boxCenter, -transform.forward, 1f, mask))//�I�𤣦A���h
                {
                    transform.position = transform.position + attackObj.transform.forward * repel * Time.deltaTime;//���h
                }
                break;
            case 1://����                
                floating_List.Add(new CharactersFloating { target = transform, force = repel, gravity = NumericalValue.gravity });//�B��List                    
                break;
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
            
            RaycastHit hit;
            //�a�O�I��
            if(OnJumpCollision_Floor(out hit, jumpRayDistance))
            {
                if (floating_List[i].force < NumericalValue.playerJumpForce / 1.35f)
                {
                    floating_List.Clear();//�M���B�ŮĪG                  
                }
            }   
        }    
    }

    /// <summary>
    /// ���O
    /// </summary>
    void OnGravity()
    {
        acceleration += 0.8f * Time.deltaTime;//�[�t��        
        transform.position = transform.position + NumericalValue.gravity * acceleration * Time.deltaTime * -Vector3.up;//���O�[�t��    
    }

    /// <summary>
    /// �I������
    /// </summary>
    void OnCollisionControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);        
        RaycastHit hit;
                
        OnCollision_Wall();//�I����_��
        OnCollision_Characters(out hit);//�I����_�}��

        //���D||���U �a���I��
        if (info.IsName("Jump") || info.IsName("Fall"))
        {          
            jumpRayDistance = info.normalizedTime < 0.3f ? jumpRayDistance = -0.1f : jumpRayDistance = 0;//�g�u����            

            if (OnJumpCollision_Floor(out hit, jumpRayDistance))
            {
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
            }          
        }

        //�@��a���I��
        if (floating_List.Count == 0)
        {
            if (OnCollision_Floor(out hit))
            {
                transform.position = transform.position + ((boxSize.y / 2) - 0.1f - hit.distance) * Vector3.up;
                acceleration = 1;//�[�t��

                if (isFall)
                {
                    isFall = false;
                    animator.SetBool("Fall", isFall);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Fall", isFall);
                }
            }
            else//�S�I��a�O
            {
                OnGravity();//���O
                OnFallJudge();//���U�P�_
                
            }
        }
        else//�B�Ū��A
        {
            OnGravity();//���O
            OnFallJudge();//���U�P�_
        }
    }

    /// <summary>
    /// �I����_�ĤH�}��
    /// </summary>    
    /// <returns></returns>
    public bool OnCollision_Enemy()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");

        if (Physics.Raycast(transform.position, transform.forward, 1.5f, mask))
        {         
            return true;
        }

        return false;
    }

    /// <summary>
    /// �I����_�}��
    /// </summary>    
    /// <returns></returns>
    public bool OnCollision_Characters(out RaycastHit hit)
    {
        LayerMask mask = LayerMask.GetMask("Player", "Enemy");
        hit = default;

        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

        //�}��I��    
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + boxCenter, new Vector3(boxCollisionDistance * collisionSize_Character, boxSize.y / 2, boxCollisionDistance * collisionSize_Character), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), boxCollisionDistance * collisionSize_Character, mask))
            {
                //�P�_�I����H���k
                //float direction = Vector3.Dot(transform.forward, Vector3.Cross(transform.position - hit.transform.position, Vector3.up)) >= 0 ? direction = 1 : direction = -1;

                //�I��
                transform.position = transform.position - rayDiration[i] * (Mathf.Abs(boxCollisionDistance * collisionSize_Character - hit.distance));
                return true;
            }         
        }

        return false;
    }

    /// <summary>
    /// �I����_��
    /// </summary>    
    /// <returns></returns>
    public bool OnCollision_Wall()
    {
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;

        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.right + transform.forward,
                                               -transform.forward,
                                               -transform.forward + transform.right,
                                               -transform.right,
                                               -transform.right -transform.forward };

        float wallHight = boxCenter.y + wallCollisionHight;//������צh�ָI��
        //����I��    
        for (int i = 0; i < rayDiration.Length; i++)
        {
            if (Physics.BoxCast(transform.position + Vector3.up * wallHight, new Vector3(boxCollisionDistance, (boxSize.y / 2) - wallCollisionHight, boxCollisionDistance), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), boxCollisionDistance + (wallCollisionDistance / 2), mask))
            {
                transform.position = transform.position - rayDiration[i] * (Mathf.Abs(boxCollisionDistance + (wallCollisionDistance / 2) - hit.distance));

                collisionObject[i] = hit.transform;//�����I������

                return true;
            }
            else
            {
                collisionObject[i] = null;//�����I������

                //�קK���~��i����
                /* if (Physics.CheckBox(transform.position + boxCenter + Vector3.up * wallHight, new Vector3(boxCollisionDistance - 0.06f, boxSize.y - (boxCenter.y + wallHight), boxCollisionDistance - 0.06f), Quaternion.Euler(transform.localEulerAngles), mask))
                 {
                     //transform.position = transform.position - rayDiration[i] * 5 * Time.deltaTime;
                 }*/
            }
        }

        return false;
    }

    /// <summary>
    /// �I����_�a��
    /// </summary>
    /// <param name="hit">RaycastHit</param>
    /// <returns></returns>
    public bool OnCollision_Floor(out RaycastHit hit)
    {
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.BoxCast(transform.position + boxCenter + Vector3.up * heightFromGround, new Vector3(boxCollisionDistance - 0.1f, 0.01f, boxCollisionDistance - 0.1f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y / 2, mask))
        {            
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���D�I����_�a��
    /// </summary>
    /// <param name="hit">RaycastHit</param>
    /// /// <param name="distance">�g�u����</param>
    /// <returns></returns>
    public bool OnJumpCollision_Floor(out RaycastHit hit, float distance)
    {
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.BoxCast(transform.position + boxCenter + Vector3.up * heightFromGround, new Vector3(boxCollisionDistance - 0.1f, 0.01f, boxCollisionDistance - 0.1f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y / 2, mask))
        {            
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���U�P�_
    /// </summary>
    void OnFallJudge()
    {        
        if (!isFall)
        {               
            if (floating_List.Count == 0)//�S���B��
            {
                if (info.IsName("Dodge"))
                {
                    if (info.normalizedTime > 0.75f)
                    {
                        if (OnFallDistance()) OnFallBehavior();
                    }
                }
                else
                {
                    if (OnFallDistance()) OnFallBehavior();
                }
            }
            else//�B�Ū��A
            {
                if (info.IsName("Jump") || info.IsName("JumpAttack") || info.IsName("Pain"))
                {
                    if (info.normalizedTime > 1)
                    {
                        if (OnFallDistance()) OnFallBehavior();
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���U�Z��
    /// </summary>
    bool OnFallDistance()
    {        
        float distance = boxSize.y * 1.2f;//���U�Z��
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.Raycast(transform.position + boxCenter, -transform.up, distance, mask))
        {            
            return false;
        }      

        Debug.DrawRay(transform.position + boxCenter, -transform.up * distance);
        return true;
    }

    /// <summary>
    /// ���U�欰
    /// </summary>
    void OnFallBehavior()
    {
        isFall = true;
        animator.SetBool("Fall", isFall);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Fall", isFall);

        if (info.IsName("Jump")) animator.SetBool("Jump", false);
        if (info.IsName("JumpAttack")) animator.SetBool("JumpAttack", false);
        if (info.IsName("Dodge")) animator.SetBool("Dodge", false);
        if (info.IsName("Pain")) animator.SetBool("Pain", false);
        if (GameDataManagement.Instance.isConnect)
        {
            PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isFall);
            PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isFall);
            PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", isFall);
            PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Pain", isFall);
        }
    }

    /// <summary>
    /// �ʵe����
    /// </summary>
    void OnAnimationOver()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

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
            if (GameDataManagement.Instance.isConnect && photonView.IsMine) PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
          
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

    public float X;
    public float Y;
    public float Z;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + boxCenter, new Vector3(boxSize.x * X, boxSize.y * Y, boxSize.z * Z));
    }
}