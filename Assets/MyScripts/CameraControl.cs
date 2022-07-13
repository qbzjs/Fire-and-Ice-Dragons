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

    Transform lookPoint;//��v���[���I
    float distance;//�P���a�Z��
    Vector3 forwardVector;//�e��V�q
    float totalVertical;//�O���������ʶq
    float limitUpAngle;//����V�W����
    float limitDownAngle;//����V�U����

    void Start()
    {        
        if(cameraControl != null)
        {
            Destroy(this);
            return;
        }
        cameraControl = this;

        lookPoint = GameObject.Find("CameraLookPoint").transform;
        distance = 2.6f;//�P���a�Z��
        forwardVector = lookPoint.forward;//�e��V�q
        limitUpAngle = 35;//����V�W����
        limitDownAngle = 20;//����V�U����
    }

    // Update is called once per frame
    void Update()
    {
        OnCameraControl();
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
        totalVertical += mouseY;//�O���������ʶq
        if (totalVertical > limitDownAngle) totalVertical = limitDownAngle;
        if (totalVertical < -limitUpAngle) totalVertical = -limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//��������b
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//�̫����V�q
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * distance;//���ʥؼ�

        //��v����ê������
        LayerMask mask = LayerMask.GetMask("StageObject");        
        if (Physics.SphereCast(lookPoint.position, 0.08f, -RotateVector, out RaycastHit hit, distance, mask))
        {
            //�I��"StageObject"��v�����ʦ�m�אּ(�[�ݪ����m - �P�I������Z��)
            moveTarget = lookPoint.position - RotateVector * hit.distance;            
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position;        
    }
}
