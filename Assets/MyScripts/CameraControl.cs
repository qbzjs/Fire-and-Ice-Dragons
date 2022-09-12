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
    static Animator playerAnimator;//�[�ݪ��aAnimator

    float totalVertical;//�O���������ʶq
    float distance;//�P���a�Z��
    bool isCollsion;//�O�_�I��

    [Header("SmoothDamp")]
    Vector3 velocity;
    [SerializeField] float smoothTime;

    [Header("�t��")]
    public float rotateSpeed;//����t��
    public float lerpSpeed;//lerp�t��
    

    [Header("���ݮɶ�")]
    [SerializeField] public float waitMoveTime; //���ݲ��ʮɶ�
    float waitTime;//���ݲ��ʮɶ�(�p�ɾ�)    
    bool isWait;//�O�_�����ݹL

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

    private void Start()
    {
        transform.position = new Vector3(285,-13, -25);//��l��m

        lerpSpeed = 4f;//����t��
        rotateSpeed = 0.80f;//����t��
        waitMoveTime = 1;//���ݲ��ʮɶ�

        //SmoothDamp
        velocity = Vector3.zero;
        smoothTime = 0.3f;
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
            playerAnimator = lookPoint.parent.GetComponent<Animator>();
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

        forwardVector = Quaternion.AngleAxis(mouseX * rotateSpeed, Vector3.up) * forwardVector;//�e��V�q

        //����W�U����
        totalVertical += mouseY * rotateSpeed;//�O���������ʶq
        if (totalVertical > NumericalValue.limitDownAngle) totalVertical = NumericalValue.limitDownAngle;
        if (totalVertical < -NumericalValue.limitUpAngle) totalVertical = -NumericalValue.limitUpAngle;

        Vector3 tempRotate = Vector3.Cross(Vector3.up, forwardVector);//��������b
        Vector3 RotateVector = Quaternion.AngleAxis(totalVertical, -tempRotate) * forwardVector;//�̫����V�q
        RotateVector.Normalize();

        Vector3 moveTarget = lookPoint.position - RotateVector * NumericalValue.distance;//���ʥؼ�
        //Vector3 moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed);//��v���a���t
        //��v����ê������        
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.SphereCast(lookPoint.position, 0.1f, -RotateVector, out RaycastHit hit, NumericalValue.distance, mask))
        {
            //lerpSpeed = 0.25f;//lerp�t��
            if (!isCollsion) isCollsion = true;
            moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed * Time.deltaTime);//��v���a���t
            //moveTarget = lookPoint.position - RotateVector * hit.distance;//��v���a��

            //�I��"StageObject"��v�����ʦ�m�אּ(�[�ݪ����m - �P�I������Z��)                                    
            /*if (distance < 0.5f && (lookPoint.transform.position - hit.transform.position).magnitude < 2.6f) moveTarget = lookPoint.position + Vector3.up * 0.49f;//�Z�����a�Ӫ�          
            else moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * hit.distance, lerpSpeed);//��v���a���t*/
        }     
        else
        {
            if (isCollsion && distance < NumericalValue.distance)//��v�����}��ê����t
            {
                moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed * Time.deltaTime);
            }
            else//�@�몬�A
            {
                //moveTarget = Vector3.Lerp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, lerpSpeed * Time.deltaTime);                
                //moveTarget = Vector3.SmoothDamp(transform.position, lookPoint.position - RotateVector * NumericalValue.distance, ref velocity, smoothTime);
            }
        }
        
        transform.position = moveTarget;
        transform.forward = lookPoint.position - transform.position; 
    }
}
