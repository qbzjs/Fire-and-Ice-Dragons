using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��v������
/// </summary>
public class CameraControl : MonoBehaviour
{
    static CameraControl cameraControl;
    public static CameraControl Instance => cameraControl;

    GameData_NumericalValue NumericalValue;

    static Transform lookPoint;//��v���[���I
    static Vector3 forwardVector;//�e��V�q
    float totalVertical;//�O���������ʶq
    float distance;//�P���a�Z��
    bool isCollsion;//�O�_�I��

    void Awake()
    {
        if (cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    private void LateUpdate()
    {
        if (lookPoint != null) OnCameraControl();
    }

    /// <summary>
    /// �]�w��v���[���I
    /// </summary>
    public static Transform SetLookPoint
    {
        set
        {
            lookPoint = value;//�]�w�[�ݪ��� 
            forwardVector = lookPoint.forward;
        }
    }

    /// <summary>
    /// ��v������
    /// </summary>
    void OnCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");//�ƹ�X�b
        float mouseY = Input.GetAxis("Mouse Y");//�ƹ�Y�b
        distance = (lookPoint.position - transform.position).magnitude;//�P���a�Z��

        forwardVector = Quaternion.AngleAxis(mouseX, Vector3.up) * forwardVector;//�e��V�q

        //����W�U����
        totalVertical += mouseY;//�O���������ʶq
        if (totalVertical > NumericalValue.limitDownAngle) totalVertical = NumericalValue.limitDownAngle;
        if (totalVertical < -NumericalValue.limitUpAngle) totalVertical = -NumericalValue.limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//��������b
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//�̫����V�q
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * NumericalValue.distance;//���ʥؼ�

        //��v����ê������
        float lerpSpeed = 0.35f;//lerp�t��
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.SphereCast(lookPoint.position, 0.1f, -RotateVector, out RaycastHit hit, NumericalValue.distance, mask))
        {
            if (!isCollsion) isCollsion = true;
            
            //�I��"StageObject"��v�����ʦ�m�אּ(�[�ݪ����m - �P�I������Z��)                                    
            if (distance < 0.5f && (lookPoint.transform.position - hit.transform.position).magnitude < 2.6f) moveTarget = lookPoint.position + Vector3.up * 0.49f;//�Z�����a�Ӫ�          
            else moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed);//��v���a���t     
        }
        else
        {
            if (isCollsion && distance < NumericalValue.distance)//��v����Ի���t
            {                
                moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed);                
                if (distance > NumericalValue.distance - 0.03f) isCollsion = false;
            }
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position;                
    }
}
