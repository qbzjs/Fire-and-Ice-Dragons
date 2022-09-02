using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Level1 : MonoBehaviour
{
    [Header("�ƭ�")]
    [SerializeField] float aroundSpeed;//��¶�t��

    [Tooltip("WayPoint")]public Transform waypoint;

    int point;

    void Start()
    {
        aroundSpeed = 23;//��¶�t��
    }
  
    void Update()
    {
        if((transform.position - waypoint.GetChild(point).transform.position).magnitude < 1)
        {
            point = OnGetNextIndex(point);            
        }

        Vector3 targetDiration = waypoint.GetChild(point).transform.position - transform.position;
        transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, 0.025f, 0.025f);
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        transform.position = transform.position + transform.forward * aroundSpeed * Time.deltaTime;
    }

    // <summary>
    /// ����`�Iposition
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public Vector3 OnGetWayPoint(int i)
    {
        return waypoint.transform.GetChild(i).position;
    }

    /// <summary>
    /// ����U�Ӹ`�I�s��
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public int OnGetNextIndex(int i)
    {
        if (i + 1 == waypoint.transform.childCount) return 0;

        return i + 1;
    }

    /// <summary>
    /// ����e�Ӹ`�I�s��
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public int OnGetPreviousIndex(int i)
    {
        if (i == 0) return waypoint.transform.childCount - 1;

        return i - 1;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoint.transform.childCount; i++)
        {
            Gizmos.color = Color.green;            
            Gizmos.DrawSphere(OnGetWayPoint(i), 0.5f);
            Gizmos.DrawLine(OnGetWayPoint(i), waypoint.transform.GetChild(OnGetNextIndex(i)).position);
        }
    }
}
