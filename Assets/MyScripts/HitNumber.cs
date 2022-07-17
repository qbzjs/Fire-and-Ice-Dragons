using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������r
/// </summary>
public class HitNumber : MonoBehaviour
{
    Canvas gameSceneUI;
    Text thisText;

    Transform target;//���˥ؼ�   
    Vector3 startPosition;//��l��m
    float lifeTime;//�ͦs�ɶ�

    void Start()
    {
        gameSceneUI = GameObject.Find("GameSceneUI").GetComponent<Canvas>();        

        transform.SetParent(gameSceneUI.transform);

        OnInitail();
    }

    
    void Update()
    {
        OnHitNumber();
    }

    /// <summary>
    /// ��l��
    /// </summary>
    void OnInitail()
    {
        lifeTime = 1.5f;//�ͦs�ɶ�
    }

    /// <summary>
    /// �]�w�ƭ�
    /// </summary>
    /// <param name="target">���˥ؼ�</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="color">��r�C��</param>
    public void OnSetValue(Transform target, float damage, Color color)
    {
        if (thisText == null) thisText = GetComponent<Text>();


        this.target = target;//���˥ؼ�
        thisText.text = damage.ToString();//����ˮ`
        startPosition = target.position + Vector3.up * 1;//��l��m
        thisText.color = color;//��r�C��        
    }

    /// <summary>
    /// ������r
    /// </summary>
    void OnHitNumber()
    {
        if (target == null) return;

        //�ͦs�ɶ�
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            OnInitail();
            gameObject.SetActive(false);
        }

        //�V�W��
        startPosition += Vector3.up * 1 * Time.deltaTime;

        Camera camera = gameSceneUI.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);

        if(gameSceneUI.renderMode == RenderMode.ScreenSpaceOverlay || camera == null)
        {
            transform.position = position;
        }
        else
        {
            Vector2 localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, camera, out localPosition);
        }
    }
}
