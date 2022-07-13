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
        
        //���󭸦�
        flyingObject.transform.position = flyingObject.transform.position + diration * speed * Time.deltaTime;

        SphereCollider box = flyingObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(flyingObject.transform.position, box.radius, layer);

        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //����������
                if (record[i] == hit.transform) return;
            }

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: flyingObject,//�����̪���
                                                                          layer: layer,
                                                                          damage: damage,
                                                                          animationName: animationName,
                                                                          effect: 0,
                                                                          repel: repel);

            record.Add(hit.transform);//�����H��������
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
   /* public void OnCollision()
    {
        SphereCollider box = flyingObject.GetComponent<SphereCollider>();
        Collider[] hits = Physics.OverlapSphere(flyingObject.transform.position, box.radius, layer);
        
        foreach (var hit in hits)
        {
            for (int i = 0; i < record.Count; i++)
            {
                //����������
                if (record[i] == hit.transform) return;                               
            }

            CharactersCollision charactersCollision = hit.GetComponent<CharactersCollision>();
            if (charactersCollision != null) charactersCollision.OnGetHit(attacker: flyingObject,//�����̪���
                                                                          layer: layer,
                                                                          damage: damage,
                                                                          animationName: animationName,
                                                                          effect: 0,
                                                                          repel: repel);

            record.Add(hit.transform);//�����H��������
        }
    }*/
}
