using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Buff�����ԩ�U
/// </summary>
public class BuffDrop : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// �������i��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        GameObject buff = eventData.pointerDrag;

        buff.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x - 10, transform.GetComponent<RectTransform>().sizeDelta.y - 10);
        buff.transform.SetParent(transform);
        buff.transform.localPosition = Vector3.zero;
    }
}
