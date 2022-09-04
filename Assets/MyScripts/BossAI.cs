using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject[] allPlayer;//�Ҧ����a    

    [Header("����")]
    GameObject AttackTarget;//������H
    int maxAttackNumber;//�֦��������ۦ�
    int activeAttackNumber;//�ϥΪ������ۦ�
    float[] attackDelayTime;//��������ɶ�(�̤p��,�̤j��)
    float attackTime;//�����ɶ�

    void Start()
    {
        allPlayer = GameObject.FindGameObjectsWithTag("Player");
        AttackTarget = allPlayer[0];

        //����
        maxAttackNumber = 1;//�֦��������ۦ�
        attackDelayTime = new float[] { 0.5f, 2.5f};//��������ɶ�(�̤p��,�̤j��)
        attackTime = 3;//�����ɶ�
    }
     
    void Update()
    {
        
    }

    /// <summary>
    /// �����ɶ�
    /// </summary>
    void OnAttackTime()
    {
        attackTime -= Time.deltaTime;//�����ɶ�
        if (attackTime <= 0)
        {
            attackTime = UnityEngine.Random.Range(attackDelayTime[0], attackDelayTime[1]);//�����ɶ�
            activeAttackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//�ϥΪ������ۦ�
        }
    }
}
