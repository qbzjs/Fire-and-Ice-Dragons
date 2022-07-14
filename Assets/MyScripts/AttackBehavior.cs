using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����欰
/// </summary>
public class AttackBehavior
{
    public Action function;    

    //�q��
    public GameObject performObject;//�������������(�ۨ�/�g�X����)    
    public LayerMask layer;//������layer
    public float damage;//�y���ˮ`
    public string animationName;//�����ĪG(�����̼��񪺰ʵe�W��)
    public float repel;//���h�Z��
    public int direction;//���h��V((0:���h 1:����))

    //��
    public Vector3 boxSize;//�񨭧�����Size

    //���{
    List<Transform> record = new List<Transform>();//�����w����������
    public float speed;//����t��
    public Vector3 diration;//�����V
    public float lifeTime;//�ͦs�ɶ�    

    /// <summary>
    /// ��Ҥ�
    /// </summary>
    public static AttackBehavior Instance => new AttackBehavior();

    /// <summary>
    /// �]�w�����ƥ�
    /// </summary>
    public void OnSetHitFunction()
    {
        function = OnHit;
    }

    /// <summary>
    /// �]�w�g���ƥ�
    /// </summary>
    public void OnSetShootFunction()
    {
        function = OnShoot;
        function += OnShootionCollision;
    }

    /// <summary>
    /// ��������(�񨭧���)
    /// </summary>
    void OnHit()
    {
        //������
        BoxCollider box = performObject.GetComponent<BoxCollider>();
        Collider[] hits = Physics.OverlapBox(performObject.transform.position + box.center + performObject.transform.forward, boxSize * performObject.transform.lossyScale.x, Quaternion.identity);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                collision.OnGetHit(attacker: performObject,//�����̪���
                                   layer: performObject.layer,//������layer
                                   damage: damage,//�y���ˮ` 
                                   animationName: animationName,//�����ĪG(����ʵe�W��)
                                   knockDirection: direction,//�����ĪG(0:���h, 1:����)
                                   repel: repel );//���h�Z��
            }
        }

        GameManagement.Instance.flyingAttackObject_List.Remove(this);
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
            performObject.SetActive(false);
            GameManagement.Instance.flyingAttackObject_List.Remove(this);
        }
   
        //�]�w�e��
        if(performObject.transform.forward != diration) performObject.transform.forward = diration;
       
        //���󭸦�
        performObject.transform.position = performObject.transform.position + performObject.transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// �I������(�g������)
    /// </summary>
    void OnShootionCollision()
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

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: performObject,//�����̪���
                                                                          layer: layer,//������layer
                                                                          damage: damage,//�y���ˮ`
                                                                          animationName: animationName,//�����ĪG(�����̼��񪺰ʵe�W��)
                                                                          knockDirection: direction,//���h��V((0:���h 1:����))
                                                                          repel: repel);//���h�Z��

            record.Add(hit.transform);//�����H��������
        }
    }
}
