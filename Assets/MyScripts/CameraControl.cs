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

        forwardVector = Quaternion.AngleAxis(mouseX, Vector3.up) * forwardVector;//�e��V�q

        //����W�U����
        /*totalVertical += mouseY;//�O���������ʶq
        if (totalVertical > NumericalValue.limitDownAngle) totalVertical = NumericalValue.limitDownAngle;
        if (totalVertical < -NumericalValue.limitUpAngle) totalVertical = -NumericalValue.limitUpAngle;*/

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//��������b
        Vector3 RotateVector = Quaternion.AngleAxis(-NumericalValue.cameraAngle, -tempRotate) * forwardVector;//�̫����V�q
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * NumericalValue.distance;//���ʥؼ�

        //��v����ê������
        LayerMask mask = LayerMask.GetMask("StageObject");        
        if (Physics.SphereCast(lookPoint.position, 0.1f, -RotateVector, out RaycastHit hit, NumericalValue.distance, mask))
        {
            //�I��"StageObject"��v�����ʦ�m�אּ(�[�ݪ����m - �P�I������Z��)
            moveTarget = lookPoint.position - RotateVector * hit.distance;            
        }
             
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position;
        //Debug.LogError(lookPoint.position.y - transform.position.y);
        //�Z�����a�Ӫ�
        if (lookPoint.position.y - transform.position.y >= 0.2f)
        {
            Debug.LogError("s");
            transform.position = lookPoint.position + lookPoint.forward * 1;
        }
    }
}
