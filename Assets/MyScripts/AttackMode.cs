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
    public GameObject performObject;//�������������(�ۨ�/�g�X����)    
    public string layer;//������layer
    public float damage;//�y���ˮ`
    public string animationName;//�����ĪG(�����̼��񪺰ʵe�W��)
    public float repel;//���h�Z��
    public int direction;//���h��V((0:���h 1:����))
    public bool isCritical;//�O�_�z��

    //��    
    public float forwardDistance;//�����d�򤤤��I�Z������e��
    public float attackRadius;//�����b�|
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
    /// �]�w�����ƥ�
    /// </summary>
    public void OnSetHitFunction()
    {        
        function = OnHit;
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
    /// ��������(�񨭧���)
    /// </summary>
    void OnHit()
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
        }   

       GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
    }   

    /// <summary>
    /// �g�X����(���{����)
    /// </summary>
    void OnShoot()
    {
        //�ͦs�ɶ�
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
            performObject.SetActive(false);
            GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
        }

        //�]�w�e��
        if(performObject.transform.forward != flightDiration) performObject.transform.forward = flightDiration;
        
        //���󭸦�
        performObject.transform.position = performObject.transform.position + performObject.transform.forward * flightSpeed * Time.deltaTime;
    }

    /// <summary>
    /// �I������_�s�����(�g������)
    /// </summary>
    void OnShootionCollision_Group()
    {
        SphereCollider sphere = performObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(performObject.transform.position, sphere.radius * sphere.transform.localScale.x);     
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
            if (collision != null)
            {
                OnSetAttackNumbericalValue(collision);
                
                //��������
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(performObject, false);
                performObject.SetActive(false);
                GameSceneManagement.Instance.AttackBehavior_List.Remove(this);
                return;
            }
        }
    }

    /// <summary>
    /// �]�w�����ƭ�
    /// </summary>
    /// <param name="charactersCollision">���������󪺸I���}��</param>
    void OnSetAttackNumbericalValue(CharactersCollision charactersCollision)
    {
        charactersCollision.OnGetHit(attacker: performObject,//�����̪���
                                     layer: layer,//������layer
                                     damage: damage,//�y���ˮ`
                                     animationName: animationName,//�����ĪG(�����̼��񪺰ʵe�W��)
                                     knockDirection: direction,//���h��V((0:���h 1:����))
                                     repel: repel,//���h�Z��
                                     isCritical: isCritical);//�O�_�z��          
    }       
}
