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
    float addSpeed;//�W�[���t��
    float randonLoseSpeed;//�üƴ�ֳt��
    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();     
        transform.SetParent(canvas_Overlay.transform);

        lifeTime = 1f;//�ͦs�ɶ�
        
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
        if (isCritical) thisText.fontSize = 75;
        else thisText.fontSize = 60;

        //�Ÿ���r
        string symbolCritical = "";
        string symbol = "";
        if (isCritical) symbolCritical = "�z��";
        if (color == Color.red || color == Color.yellow) symbol = "-";
        if (color == Color.green) symbol = "+";
        symbol = symbolCritical + symbol;

        //��r
        this.target = target;//���˥ؼ�
        thisText.text = symbol + Mathf.Round(damage).ToString();//����ˮ`(�|�ˤ��J)        
        thisText.color = color;//��r�C�� 
        addSpeed = UnityEngine.Random.Range(10.5f, 12.5f); ;//�W�[���t��
        randonLoseSpeed = UnityEngine.Random.Range(47.0f, 57.5f);//�üƴ�ֳt��        
    }
    
    /// <summary>
    /// ������r�欰
    /// </summary>
    void OnHitNumberBehavior()
    {
        if (target == null) return;

        //�W�L�Z�������        
        if((target.position - Camera.main.transform.position).magnitude > 40) Destroy(gameObject);

        if (addSpeed > 0)
        {
            addSpeed -= randonLoseSpeed * Time.deltaTime;
            if (addSpeed <= 0) addSpeed = 0;
        }

        speed += addSpeed * Time.deltaTime; 

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
