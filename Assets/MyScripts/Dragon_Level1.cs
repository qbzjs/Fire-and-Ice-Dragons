using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Level1 : MonoBehaviour
{
    [SerializeField] Transform rotateAroundTarger;//��¶�ؼ�
    public Transform SetRotateAroundTarger { set { rotateAroundTarger = value; } }

    [Header("�ƭ�")]
    [SerializeField] float aroundSpeed;//��¶�t��
    [SerializeField] float distance;//�Z�����a�Z��

    void Start()
    {
        aroundSpeed = 5;//��¶�t��
        distance = 20;//�Z�����a�Z��
    }
  
    void Update()
    {
        if (rotateAroundTarger != null)
        {
            Vector3 thisPos = transform.position;
            thisPos.y = 0;
            Vector3 targetPos = rotateAroundTarger.position;
            targetPos.y = 0;
            Vector3 forward = thisPos - targetPos;

            //transform.right = forward;
            
            
            /*if((thisPos - targetPos).magnitude > distance)
            {
                transform.position = transform.position - transform.right * aroundSpeed / 2 * Time.deltaTime;
            }*/               
        }

        transform.position = transform.position + transform.forward * aroundSpeed * Time.deltaTime;
    }
}
