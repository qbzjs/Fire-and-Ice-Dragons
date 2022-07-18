using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar_Characters : MonoBehaviour
{
    Canvas canvas_World;
    float hpProportion;//�ͩR���
    Transform target;//�ؼЪ���
    Image lifeBarFront_Image;//�ͩR��(�e)
    Image lifeBarMid_Image;//�ͩR��(��)

    void Start()
    {
        hpProportion = 1;
        lifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarFront_Image");
        lifeBarFront_Image.fillAmount = hpProportion;
        lifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarMid_Image");
        lifeBarMid_Image.fillAmount = hpProportion;        
    }
    
    void Update()
    {
        OnLifeBarBehavior();
    }

    /// <summary>
    /// �]�w�ؼЪ���
    /// </summary>
    public Transform SetTarget 
    { 
        set
        {
            target = value;
            canvas_World = GameObject.Find("Canvas_World").GetComponent<Canvas>();
            transform.SetParent(canvas_World.transform); 
        }
    }

    /// <summary>
    /// �]�w�ƭ�
    /// </summary>
    public float SetValue { set { hpProportion = value; } }
    
    /// <summary>
    /// �ͩR���欰
    /// </summary>
    void OnLifeBarBehavior()
    {
        if (target == null) return;

        //���H�ؼ�
        Camera cnavasCamera = canvas_World.worldCamera;
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        transform.rotation = cnavasCamera.transform.rotation;

        //�ͩR���欰
        if (hpProportion <= 0) hpProportion = 0;//�ͩR���                
        lifeBarFront_Image.fillAmount = hpProportion;//�ͩR��(�e)        
        if (lifeBarFront_Image.fillAmount < lifeBarMid_Image.fillAmount)//�ͩR��(��)
        {
            lifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }
    }
}
