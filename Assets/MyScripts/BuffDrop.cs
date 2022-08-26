using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Buff�����ԩ�U
/// </summary>
public class BuffDrop : MonoBehaviour, IDropHandler
{  
    public string buffBoxName;

    /// <summary>
    /// �������i��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {        
        //�w�g��Buff�b�̭�
        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            Transform childOriginalParent = child.GetComponent<BuffButtonDrag>().originalParent;
            child.SetParent(childOriginalParent);
            child.localPosition = Vector3.zero;
            child.GetComponent<RectTransform>().sizeDelta = new Vector2(childOriginalParent.GetComponent<RectTransform>().sizeDelta.x - 30, childOriginalParent.GetComponent<RectTransform>().sizeDelta.y - 30);
        }

        GameObject buff = eventData.pointerDrag;
        buff.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x - 30, transform.GetComponent<RectTransform>().sizeDelta.y - 30);
        buff.transform.SetParent(transform);
        buff.transform.localPosition = Vector3.zero;        
    }

    /// <summary>
    /// �ˬd�˳ƪ�Buff
    /// </summary>
    public void OnCheckBuff()
    {
        if (transform.childCount <= 0)
        {
            StartSceneUI.Instance.OnSetEquipBuff(boxName: buffBoxName, buff: 0);
            return;
        }

        int equipBuff = transform.GetChild(0).GetComponent<BuffButtonDrag>().buffAble;
        StartSceneUI.Instance.OnSetEquipBuff(boxName: buffBoxName, buff: equipBuff);
    }
}
