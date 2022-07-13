using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������
/// </summary>
public class FlyingAttackObject
{
    public GameObject flyingObject;//���檫��
    public float speed;//����t��
    public Vector3 diration;//�����V
    public float lifeTime;//�ͦs�ɶ�
    public LayerMask layer;//������layer
    public float damage;//�y���ˮ`
    public string animationName;//�����ĪG(�����̼��񪺰ʵe�W��)
    public float repel;//���h�Z��
   
    List<Transform> record = new List<Transform>();//�����w����������

    //����
    public void OnFlying()
    {
        //�ͦs�ɶ�
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {         
            flyingObject.SetActive(false);
            GameManagement.Instance.flyingAttackObject_List.Remove(this);
        }
        
        //�]�w�e��
        if(flyingObject.transform.forward != diration) flyingObject.transform.forward = diration;
       
        //���󭸦�
        flyingObject.transform.position = flyingObject.transform.position + flyingObject.transform.forward * speed * Time.deltaTime;
     
        OnCollision();
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void OnCollision()
    {
        SphereCollider sphere = flyingObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(flyingObject.transform.position, sphere.radius * sphere.transform.localScale.x);     
        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //����������
                if (record[i] == hit.transform) return;
            }

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: flyingObject,//�����̪���
                                                                          layer: layer,//������layer
                                                                          damage: damage,//�y���ˮ`
                                                                          animationName: animationName,//�����ĪG(�����̼��񪺰ʵe�W��)
                                                                          effect: 0,//���h��V((0:���h 1:����))
                                                                          repel: repel);//���h�Z��

            record.Add(hit.transform);//�����H��������
        }
    }
}
