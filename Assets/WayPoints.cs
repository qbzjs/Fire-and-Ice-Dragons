using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    static WayPoints wayPoints;
    public static WayPoints Instace => wayPoints;

    const float radius = 0.5f;

    //�Ҧ��`�I��m
    Vector3[] nodesPosition;
    public Vector3[] GetNodesPosition => nodesPosition;

    private void Awake()
    {
        if (wayPoints != null)
        {
            Destroy(this);
            return;
        }
        wayPoints = this;
    }

    private void Start()
    {
        nodesPosition = new Vector3[transform.childCount];

        OnSaveNode();
    }

    /// <summary>
    /// �`�I�s��
    /// </summary>
    void OnSaveNode()
    {
        //�Ыؤ�r�ɬ���(�л\)
        //StreamWriter streamWriter = new StreamWriter("Assets/AiNode.txt", false);

        //string s = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            /*s = "";
            s += transform.GetChild(i).name;//�`�I����W��
            s += " ";
            s += transform.GetChild(OnGetNextIndex(i)).name;//�U�Ӹ`�I����W��
            s += " ";
            s += transform.GetChild(OnGetPreviousIndex(i)).name;//�e�Ӹ`�I����W��

            streamWriter.WriteLine(s);*/

            nodesPosition[i] = transform.GetChild(i).position;//�����`�I��m
        }

        //OnSetNeighbor();
        //streamWriter.Close();
    }

    /// <summary>
    /// ����`�Iposition
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public Vector3 OnGetWayPoint(int i)
    {
        return transform.GetChild(i).position;
    }

    /// <summary>
    /// ����U�Ӹ`�I�s��
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public int OnGetNextIndex(int i)
    {
        if (i + 1 == transform.childCount) return 0;

        return i + 1;
    }

    /// <summary>
    /// ����e�Ӹ`�I�s��
    /// </summary>
    /// <param name="i">�`�I�s��</param>
    /// <returns></returns>
    public int OnGetPreviousIndex(int i)
    {
        if (i == 0) return transform.childCount - 1;

        return i - 1;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(OnGetWayPoint(i), radius);
            Gizmos.DrawLine(OnGetWayPoint(i), transform.GetChild(OnGetNextIndex(i)).position);
        }
    }
}
