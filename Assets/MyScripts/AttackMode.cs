using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ҧ�
/// </summary>
public class AttackMode
{
    public Action function;

    //�q��
    public GameObject performCharacters;//��������}��
    public GameObject performObject;//�������������(�ۨ�/�g�X����)    
    public string layer;//������layer
    public float damage;//�y���ˮ`(�v���q(%))
    public string animationName;//�����ĪG(�����̼��񪺰ʵe�W��)
    public float repel;//���h�Z��
    public int direction;//���h��V((0:���h 1:����))
    public bool isCritical;//�O�_�z��

    //��    
    public float forwardDistance;//�����d�򤤤��I�Z������e��
    public float attackRadius;//�����b�|(���)
    public Vector3 attackRange;//�����d��(���)
    public bool isAttackBehind;//�O�_�����I��ĤH

    //���{
    List<Transform> record = new List<Transform>();//�����w����������
    public float flightSpeed;//����t��
    public Vector3 flightDiration;//�����V
    public float lifeTime;//�ͦs�ɶ�    

    /// <summary>
    /// ��Ҥ�
    /// </summary>
    public static AttackMode Instance => new AttackMode();

    /// <summary>
    /// �]�w�v���ƥ�
    /// </summary>
    public void OnSetHealFunction()
    {
        function = OnHeal;
    }

    /// <summary>
    /// �]�w�����ƥ�(��νd��)
    /// </summary>
    public void OnSetHitSphereFunction()
    {        
        function = OnHitSphere;
    }

    /// <summary>
    /// �]�w�����ƥ�(��νd��)
    /// </summary>
    public void OnSetHitBoxFunction()
    {
        function = OnHitBox;
    }

    /// <summary>
    /// �]�w�g���ƥ�_�s�����
    /// </summary>
    public void OnSetShootFunction_Group()
    {
        function = OnShoot;
        function += OnShootionCollision_Group;
    }

    /// <summary>
    /// �]�w�g���ƥ�_�������
    /// </summary>
    public void OnSetShootFunction_Single()
    {        
        function = OnShoot;
        function += OnShootionCollision_Single;        
    }

    /// <summary>
    /// �]�w�w�I����ˮ`
    /// </summary>
    public void OnSetContinuedFunction()
    {
        function = OnShoot;
        function += OnContinuedCollision;
    }

    /// <summary>
    /// �v��
    /// </summary>
    void OnHeal()
    {
        BoxCollider box = performObject.GetComponent<BoxCollider>();

        Collider[] hits = Physics.OverlapSphere(performObject.transform.position + box.center + performObject.transform.forward * forwardDistance, attackRadius);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                //�O�_�����I��ĤH
                if (!isAttackBehind && Vector3.Dot(performObject.transform.forward, hit.transform.position - performObject.transform.position) <= 0) continue;
                OnSetHealNumbericalValue(collision);
            }
        }

        GameSceneManagement.Instance.AttackMode_List.Remove(this);
    }

    /// <summary>
    /// ��������(�񨭧���(��Χ����d��))
    /// </summary>
    void OnHitSphere()
    {
        BoxCollider box = performObject.GetComponent<BoxCollider>();
        
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position + box.center + performObject.transform.forward * forwardDistance, attackRadius);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {                
                //�O�_�����I��ĤH
                if (!isAttackBehind && Vector3.Dot(performObject.transform.forward, hit.transform.position - performObject.transform.position) <= 0) continue;
                OnSetAttackNumbericalValue(collision);                
            }

            //���I����
            Stronghold stronghold = hit.GetComponent<Stronghold>();
            if (stronghold != null) stronghold.OnGetHit(layer, damage);
        }   

       GameSceneManagement.Instance.AttackMode_List.Remove(this);
    }

    /// <summary>
    /// ��������(�񨭧���(��Χ����d��))
    /// </summary>
    void OnHitBox()
    {
        BoxCollider box = performObject.GetComponent<BoxCollider>();
        
        Collider[] hits = Physics.OverlapBox(performObject.transform.position + box.center + performObject.transform.forward * forwardDistance, attackRange, Quaternion.Euler(performObject.transform.localEulerAngles));
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                //�O�_�����I��ĤH
                if (!isAttackBehind && Vector3.Dot(performObject.transform.forward, hit.transform.position - performObject.transform.position) <= 0) continue;
                OnSetAttackNumbericalValue(collision);
            }

            //���I����
            Stronghold stronghold = hit.GetComponent<Stronghold>();
            if (stronghold != null) stronghold.OnGetHit(layer, damage);
        }

        GameSceneManagement.Instance.AttackMode_List.Remove(this);
    }    

    /// <summary>
    /// �g�X����(���{����)
    /// </summary>
    void OnShoot()
    {        
        lifeTime -= Time.deltaTime;//�ͦs�ɶ�
        
        //�ͦs�ɶ� || �I�����
        if (lifeTime <= 0)
        {
            if (layer != "Boss")
            {                
                if (Physics.CheckSphere(performObject.transform.position, performObject.GetComponent<SphereCollider>().radius, 1 << LayerMask.NameToLayer("StageObject")))
                {
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
                    performObject.SetActive(false);
                    GameSceneManagement.Instance.AttackMode_List.Remove(this);
                }
            }

            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
            performObject.SetActive(false);
            GameSceneManagement.Instance.AttackMode_List.Remove(this);
        }

        //�]�w�e��
        if (layer != "Boss")
        {
            if (performObject.transform.forward != flightDiration) performObject.transform.forward = flightDiration;                        
        }

        //���󭸦�
        performObject.transform.position = performObject.transform.position + performObject.transform.forward * flightSpeed * Time.deltaTime;
    }

    /// <summary>
    /// �I������_�s�����(�g������)
    /// </summary>
    void OnShootionCollision_Group()
    {
        SphereCollider sphere = performObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position + sphere.center, sphere.radius);     
        foreach (var hit in hits)
        {            
            for (int i = 0; i < record.Count; i++)
            {
                //����������
                if (record[i] == hit.transform)
                {                    
                    return;
                }
            }

            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                OnSetAttackNumbericalValue(collision);               
                record.Add(hit.transform);//�����H��������                
            }

            //���I����
            Stronghold stronghold = hit.GetComponent<Stronghold>();
            if (stronghold != null)
            {
                stronghold.OnGetHit(layer, damage);
                record.Add(hit.transform);//�����H��������
            }
        }
    }        

    /// <summary>
    /// �I������_�������(�g������)
    /// </summary>
    void OnShootionCollision_Single()
    {
        SphereCollider sphere = performObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position, sphere.radius * sphere.transform.localScale.x);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null && collision.gameObject.layer != LayerMask.NameToLayer(layer))
            {
                OnSetAttackNumbericalValue(collision);
                
                //��������
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
                performObject.SetActive(false);
                GameSceneManagement.Instance.AttackMode_List.Remove(this);
                return;
            }

            //���I����
            Stronghold stronghold = hit.GetComponent<Stronghold>();
            if (stronghold != null) stronghold.OnGetHit(layer, damage);
        }
    }

    /// <summary>
    /// �I������_�w�I�������
    /// </summary>
    void OnContinuedCollision()
    {
        BoxCollider box = performObject.GetComponent<BoxCollider>();
        Collider[] hits = Physics.OverlapBox(performObject.transform.position, new Vector3(box.size.x, box.size.y , box.size.z ), Quaternion.Euler(0, 90, 0));
        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //����������
                if (record[i] == hit.transform) return;
            }

            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                OnSetAttackNumbericalValue(collision);
                record.Add(hit.transform);//�����H��������                
            }

            //���I����
            Stronghold stronghold = hit.GetComponent<Stronghold>();
            if (stronghold != null) stronghold.OnGetHit(layer, damage);
        }
    }     

    /// <summary>
    /// �]�w�����ƭ�
    /// </summary>
    /// <param name="charactersCollision">���������󪺸I���}��</param>
    void OnSetAttackNumbericalValue(CharactersCollision charactersCollision)
    {
        charactersCollision.OnGetHit(attacker: performCharacters,//��������}��
                                     attackerObject: performObject,//�����̪���
                                     layer: layer,//������layer
                                     damage: damage,//�y���ˮ`
                                     animationName: animationName,//�����ĪG(�����̼��񪺰ʵe�W��)
                                     knockDirection: direction,//���h��V((0:���h 1:����))
                                     repel: repel,//���h�Z��
                                     isCritical: isCritical);//�O�_�z��          
    }

    /// <summary>
    /// �]�w�v���ƭ�
    /// </summary>
    /// <param name="charactersCollision">���v�����󪺸I���}��</param>
    void OnSetHealNumbericalValue(CharactersCollision charactersCollision)
    {
        charactersCollision.OnGetHeal(attacker: performObject,//�����̪���
                                     layer: layer,//������layer
                                     heal: damage,//�y���ˮ`                                   
                                     isCritical: isCritical);//�O�_�z��          
    }
}
