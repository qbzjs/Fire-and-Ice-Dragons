using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������r
/// </summary>
public class HitNumber : MonoBehaviour
{
    Canvas canvas_Overlay;
    Text thisText;

    [SerializeField]Transform target;//���˥ؼ�   
    Vector3 startPosition;//��l��m
    float lifeTime;//�ͦs�ɶ�
    float speed;//�t��
    float colorAlpha;//�z����

    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();     
        transform.SetParent(canvas_Overlay.transform);

        lifeTime = 1.5f;//�ͦs�ɶ�
    }

    
    void Update()
    {
        OnHitNumberBehavior();
    }

    /// <summary>
    /// �]�w�ƭ�
    /// </summary>
    /// <param name="target">���˥ؼ�</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="color">��r�C��</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnSetValue(Transform target, float damage, Color color, bool isCritical)
    {
        if (thisText == null) thisText = GetComponent<Text>();

        //�z���r��j
        if (isCritical) thisText.fontSize = 105;
        else thisText.fontSize = 70;

        this.target = target;//���˥ؼ�
        thisText.text = Mathf.Round(damage).ToString();//����ˮ`(�|�ˤ��J)        
        thisText.color = color;//��r�C��
        colorAlpha = color.a;
    }
    
    /// <summary>
    /// ������r�欰
    /// </summary>
    void OnHitNumberBehavior()
    {
        if (target == null) return;

        speed += 2 * Time.deltaTime; 

        //��r����
        startPosition = target.position + target.transform.up * (1 + speed);
        //��r�z����
        thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, lifeTime);

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);        

        //�P�_Canvas��RenderMode
        if (canvas_Overlay.renderMode == RenderMode.ScreenSpaceOverlay || camera == null)
        {
            transform.position = position;
        }
        else
        {
            Vector2 localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, camera, out localPosition);
        }

        //�ͦs�ɶ�
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 || position.z < 0)
        {
            Destroy(gameObject);
        }        
    }
}
