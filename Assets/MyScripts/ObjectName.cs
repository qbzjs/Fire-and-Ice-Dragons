using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectName : MonoBehaviour
{
    Canvas canvas_Overlay;
    Text thisText;

    PlayerControl playerControl;

    Transform theTarget;//�ؼ�   
    Vector3 startPosition;//��l��m
    float postitionHight;//����

    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();

        //thisText = GetComponent<Text>();
        transform.SetParent(canvas_Overlay.transform);
        //target = transform.parent;

    }

    void Update()
    {
        OnBehavior();
    }

    /// <summary>
    /// �]�w�W��
    /// </summary>
    /// <param name="target">�ؼЪ���</param>
    /// <param name="thisName">�W��</param>
    /// <param name="thisColor">�C��</param>
    /// <param name="hight">����</param>
    public void OnSetName(Transform target, string thisName, Color thisColor, float hight )
    {
        if (thisText == null) thisText = GetComponent<Text>();

        theTarget = target;
        thisText.text = thisName;
        thisText.color = thisColor;
        postitionHight = hight;//����

        PlayerControl[] PC = GameObject.FindObjectsOfType<PlayerControl>();

        for (int i = 0; i < PC.Length; i++)
        {
            if (PC[i].enabled)
            {
                playerControl = PC[i];
                break;
            }
        }
    }

    /// <summary>
    /// �欰
    /// </summary>
    void OnBehavior()
    {
        if (theTarget == null) return;
        if (!theTarget.gameObject.activeSelf) Destroy(gameObject);

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);
        int maxSize = 33;

        startPosition = theTarget.position + theTarget.transform.up * postitionHight;
        int size = (int)(400 / (theTarget.position - playerControl.transform.position).magnitude);
        if (size <= 0) size = maxSize;
        if (size >= maxSize) size = maxSize;
        thisText.fontSize = size;

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


        //�P���a��������ê��
        if (Physics.Linecast(theTarget.position + Vector3.up * 0.5f, playerControl.transform.position + Vector3.up * 0.5f, 1 << LayerMask.NameToLayer("StageObject"))
            || GameSceneUI.Instance.isOptions
            || GameSceneUI.Instance.isGameOver
            || Vector3.Dot(Camera.main.transform.forward, theTarget.transform.position - Camera.main.transform.position) < 0)
        {
            if (thisText.enabled) thisText.enabled = false;
        }
        else
        {
            if (!thisText.enabled) thisText.enabled = true;
        }


    }
}
