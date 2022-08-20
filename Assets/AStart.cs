using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart
{
    public AStart Instance;

    WayPoints wayPoints;
    Vector3[] nodes;//����Ҧ����`�I
    List<Vector3> pathNodes = new List<Vector3>();//�������|�I

    /// <summary>
    /// ��l
    /// </summary>
    public void initial()
    {
        Instance = this;
        wayPoints = WayPoints.Instace;
    }

    /// <summary>
    /// �M��̨θ��u
    /// </summary>
    /// <param name="startPoint">�}�l��m</param>
    /// <param name="targetPosition">�ؼЦ�m</param>
    /// <returns></returns>
    public List<Vector3> OnGetBestPoint(Vector3 startPoint, Vector3 targetPosition)
    {
        pathNodes.Clear();
        pathNodes.Add(startPoint);//��l���|�I

        nodes = wayPoints.GetNodesPosition;//����Ҧ����`�I

        float distance = 10000;//�Z��
        int closeNumber = 0;//�̪񪺸`�I�s��

        #region �Ĥ@�B:�M��Z���_�l�I�̪񪺸`�I
        for (int i = 0; i < nodes.Length; i++)
        {
            float p = (startPoint - nodes[i]).magnitude;//�Z��

            //����ê�����L
            if (Physics.Linecast(startPoint, nodes[i], 1 << LayerMask.NameToLayer("StageObject")))
            {                
                continue;
            }

            //�M��̪񪺶Z��
            if (p < distance)
            {
                distance = p;
                closeNumber = i;
            }
        }
        #endregion

        pathNodes.Add(nodes[closeNumber]);//�����̪񪺸`�I

        #region �ĤG�B:�M��̪�`�I���F�~          

        float closestNode = (targetPosition - nodes[closeNumber]).magnitude;//�̪��I����I�Z��
        float nextNeighbor = (targetPosition - nodes[(wayPoints.OnGetNextIndex(closeNumber))]).magnitude;//�̪��I�F�~����I�Z��(�U�ӽs��)
        float previousNeighbor = (targetPosition - nodes[(wayPoints.OnGetPreviousIndex(closeNumber))]).magnitude;//�̪��I�F�~����I�Z��(�e�ӽs��)        

        bool isNext = false;//�P�_�s�����V
        int number;//�`�I�s��
        if (nextNeighbor < previousNeighbor)
        {
            isNext = true;//�P�_�s�����V
            pathNodes.Add(nodes[wayPoints.OnGetNextIndex(closeNumber)]);//�������|�I

            if (OnJudegNextClosePoint(closeNumber, nodes, targetPosition)) return pathNodes;
        }
        else
        {
            pathNodes.Add(nodes[wayPoints.OnGetPreviousIndex(closeNumber)]);//�������|�I

            if (OnJudegPreviousClosePoint(closeNumber, nodes, targetPosition)) return pathNodes;
        }
        #endregion

        #region �ĤT�B:�P�_���᪺�`�I
        number = closeNumber;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (isNext)
            {
                number = wayPoints.OnGetNextIndex(number);

               //�P�_�O�_����
                /*if (Physics.Linecast(startPoint, nodes[number], 1 << LayerMask.NameToLayer("StageObject")))
                {
                    Debug.LogError("a");
                    pathNodes.Add(nodes[number]);//�������|�I;
                    //number = wayPoints.OnGetNextIndex(number);
                    continue;
                }*/

                pathNodes.Add(nodes[wayPoints.OnGetNextIndex(number)]);//�������|�I
                if (OnJudegNextClosePoint(number, nodes, targetPosition)) return pathNodes;
            }
            else
            {
                number = wayPoints.OnGetPreviousIndex(number);

                //�P�_�O�_����
                /*if (Physics.Linecast(startPoint, nodes[number], 1 << LayerMask.NameToLayer("StageObject")))
                {

                    Debug.LogError("a");
                    pathNodes.Add(nodes[wayPoints.OnGetNextIndex(number)]);//�������|�I;
                    continue;
                }*/

                pathNodes.Add(nodes[number]);//�������|�I
                if (OnJudegPreviousClosePoint(number, nodes, targetPosition)) return pathNodes;
            }
        }
        #endregion

        return pathNodes;
    }  

    /// <summary>
    /// �P�_�̪�Z��(Next)
    /// </summary>
    /// <param name="number">�`�I�s��</param>
    /// <param name="nodes">�Ҧ����`�I</param>
    /// <param name="targetPosition">�ؼЦ�m</param>
    /// <returns></returns>
    bool OnJudegNextClosePoint(int number, Vector3[] nodes, Vector3 targetPosition)
    {
        int next = wayPoints.OnGetNextIndex(number);
        float n1 = (targetPosition - nodes[wayPoints.OnGetNextIndex(number)]).magnitude;
        float n2 = (targetPosition - nodes[wayPoints.OnGetNextIndex(next)]).magnitude;

        if (n1 < n2 && !Physics.Linecast(nodes[number], targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
        {
            pathNodes.Add(targetPosition);//�������|�I
            return true;
        }

        return false;
    }

    /// <summary>
    /// �P�_�̪�Z��(Previous)
    /// </summary>
    /// <param name="number">�`�I�s��</param>
    /// <param name="nodes">�Ҧ����`�I</param>
    /// <param name="targetPosition">�ؼЦ�m</param>
    /// <returns></returns>
    bool OnJudegPreviousClosePoint(int number, Vector3[] nodes, Vector3 targetPosition)
    {
        int next = wayPoints.OnGetPreviousIndex(number);
        float n1 = (targetPosition - nodes[wayPoints.OnGetPreviousIndex(number)]).magnitude;
        float n2 = (targetPosition - nodes[wayPoints.OnGetPreviousIndex(next)]).magnitude;

        if (n1 < n2 && !Physics.Linecast(nodes[number], targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
        {
            pathNodes.Add(targetPosition);//�������|�I
            return true;
        }

        return false;
    }
}
