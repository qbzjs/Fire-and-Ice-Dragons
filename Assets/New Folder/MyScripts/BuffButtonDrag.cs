using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Buff������
/// </summary>
public class BuffButtonDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;//��l������
    public Vector2 originalMousePosition;//�ƹ��I����m
    public int buffAble;//Buff��O�s��

    void Start()
    {
        originalParent = transform.parent;
    }

    /// <summary>
    /// �}�l���
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        if (buff == null) return;

        buff.GetComponent<Image>().raycastTarget = false;
        buff.transform.SetParent(StartSceneUI.Instance.transform);
        originalMousePosition = eventData.position - (Vector2)buff.transform.position;              
    }

    /// <summary>
    /// ���
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        buff.transform.position = eventData.position - originalMousePosition;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;
        if (buff == null) return;

        buff.GetComponent<Image>().raycastTarget = true;

        //�S�ԶiBuff��
        if (buff.transform.parent == StartSceneUI.Instance.transform)
        {
            buff.GetComponent<RectTransform>().sizeDelta = new Vector2(originalParent.GetComponent<RectTransform>().sizeDelta.x - 10, originalParent.GetComponent<RectTransform>().sizeDelta.y - 10);
            buff.transform.SetParent(originalParent);//�^���
            buff.transform.localPosition = Vector3.zero;
        }

        //�ˬd�˳Ƥ���Buff
        StartSceneUI.Instance.buffBox_1.OnCheckBuff();
        StartSceneUI.Instance.buffBox_2.OnCheckBuff();
    }
}
