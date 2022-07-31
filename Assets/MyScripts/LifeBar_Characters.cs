using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar_Characters : MonoBehaviour
{
    Canvas canvas_World;
    float hpProportion;//�ͩR���
    Transform target;//�ؼЪ���
    [SerializeField]Image lifeBarFront_Image;//�ͩR��(�e)
    Image lifeBarMid_Image;//�ͩR��(��)
    Image lifeBarBack_Image;//�ͩR��(��)
    float targetHight;//���󰪫�

    void Start()
    {
        hpProportion = 1;
        
        lifeBarFront_Image.fillAmount = hpProportion;
        lifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarMid_Image");//�ͩR��(��)
        lifeBarMid_Image.fillAmount = hpProportion;
        lifeBarBack_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LifeBarBack_Image");//�ͩR��(��)
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
            targetHight = target.GetComponent<BoxCollider>().size.y / 5f;
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
        if (target == null)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }
               
        //���H�ؼ�
        Camera cnavasCamera = canvas_World.worldCamera;
        transform.position = new Vector3(target.position.x, target.position.y + targetHight, target.position.z);
        transform.rotation = cnavasCamera.transform.rotation;
        
        //�ͩR���欰
        if (hpProportion <= 0) hpProportion = 0;//�ͩR���                
        lifeBarFront_Image.fillAmount = hpProportion;//�ͩR��(�e)        
        if (lifeBarFront_Image.fillAmount < lifeBarMid_Image.fillAmount)//�ͩR��(��)
        {
            lifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }

        //��������
        if (lifeBarMid_Image.fillAmount <= 0)
        {
            lifeBarFront_Image.enabled = false;//�ͩR��(�e)
            lifeBarMid_Image.enabled = false;//�ͩR��(��)
            lifeBarBack_Image.enabled = false;//�ͩR��(��)
        }

        //�}�Ҫ���
        if(lifeBarFront_Image.fillAmount > 0 && !lifeBarFront_Image.enabled)
        {
            lifeBarFront_Image.enabled = true;//�ͩR��(�e)
            lifeBarMid_Image.enabled = true;//�ͩR��(��)
            lifeBarBack_Image.enabled = true;//�ͩR��(��)
        }
    }
}
