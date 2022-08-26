using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart
{
    public AStart Instance;

    WayPoints wayPoints;
    NodePath[] allNodes;//����Ҧ����`�I
    List<Vector3> pathNodesList = new List<Vector3>();//�������|�I
    List<NodePath> closeNodeList = new List<NodePath>();//�����w�������`�I

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
        pathNodesList.Clear();//�������|�I
        closeNodeList.Clear();//�����w�������`�I
        pathNodesList.Add(startPoint);//��l���|�I

        allNodes = wayPoints.GetNodePaths;//����Ҧ����`�I

        NodePath node = null;

        //���s�Ҧ��`�I���A
        for (int i = 0; i < allNodes.Length; i++)
        {
            allNodes[i].nodeState = NodePath.NodeState.�}��;
        }

        float distance = 10000;//�Z��
        int closeNumber = 0;//�̪񪺸`�I�s��

        #region �Ĥ@�B:�M��Z���_�l�I�̪񪺸`�I
        for (int i = 0; i < allNodes.Length; i++)
        {
            float closestDistance = (startPoint - allNodes[i].transform.position).magnitude;//�_�I��`�I�Z��

            //����ê�����L
            if (Physics.Linecast(startPoint, allNodes[i].transform.position, 1 << LayerMask.NameToLayer("StageObject")))
            {
                continue;
            }

            //�M��̪񪺶Z��
            if (closestDistance < distance)
            {
                distance = closestDistance;
                closeNumber = i;
            }
        }

        node = allNodes[closeNumber];//�̪�`�I
                                     
        //����F�~�`�I
        //node = OnCompareStartNeighborNode(node: node, targetPosition: targetPosition, startPoint: startPoint, distance: distance);

        node.nodeState = NodePath.NodeState.����;//�`�I���A
        closeNodeList.Add(node);//�����w�������`�I
        pathNodesList.Add(node.transform.position);//��l���|�I

        //�P�_�O�_���ؼ��I
        if (node.transform.position == targetPosition)
        {
            pathNodesList.Add(targetPosition);//�����ؼ��I
            return pathNodesList;//�^�ǩҦ��������|�I
        }

        #endregion


        #region �ĤG�B:����̪�`�I�F�~
        float bestDistance = 10000;//�̨ζZ��                
        int bestNeighbor = 0;//�̪񪺾F�~�s��
        while (closeNodeList.Count < allNodes.Length)
        {
            bool isHaveBestNode = false;//�O�_����񪺸`�I
            bestNeighbor = 0;//�̪񪺾F�~�s��
            for (int i = 0; i < node.neighborNode.Length; i++)
            {
                if (node.neighborNode[i].nodeState == NodePath.NodeState.����) continue;               
                Vector3 nextPosition = node.transform.position;//�U�Ӹ`�I��m
                Vector3 neighborPosition = node.neighborNode[i].transform.position;//�F�~�`�I��m

                float G = (nextPosition - neighborPosition).magnitude;//��U�Ӹ`�I��m
                float H = (neighborPosition - targetPosition).magnitude;//�U�Ӹ`�I��ؼЦ�m
                float F = G + H;//�Z��

                if (F < bestDistance)
                {
                    isHaveBestNode = true;//����񪺸`�I
                    bestDistance = F;//�̨ζZ��
                    bestNeighbor = i;//�̪񪺾F�~�s��                    
                }
            }            

            //�P�_�O�_���ؼ��I
            if (node.neighborNode[bestNeighbor].transform.position == targetPosition)
            {
                pathNodesList.Add(targetPosition);//�����ؼ��I
                return pathNodesList;//�^�ǩҦ��������|�I
            }            

            #region �ĤT�B:�̪�`�I�O�_���I��
            if (!isHaveBestNode)//�S����񪺸`�I
            {                
                bestDistance = 10000;
                //�P�_�P�ؼи��|�O�_����ê��
                if (Physics.Linecast(node.transform.position, targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
                {
                    Debug.LogError("s");
                    for (int j = 0; j < allNodes.Length; j++)
                    {
                        //�s�U�ثe�`�I
                        node.nodeState = NodePath.NodeState.����;//�`�I���A
                        closeNodeList.Add(node);//�����w�������`�I
                        pathNodesList.Add(node.transform.position);//�����`�I

                        //����F�~�`�I
                        
                        bool isHaveBestNodeForNext = false;//�O�_����񪺸`�I
                        int bestNeighborForNext = 0;//�̪񪺾F�~�s��
                        for (int i = 0; i < node.neighborNode.Length; i++)
                        {
                            if (node.neighborNode[i].nodeState == NodePath.NodeState.����) continue;

                            Vector3 nextPosition = node.transform.position;//�U�Ӹ`�I��m
                            Vector3 neighborPosition = node.neighborNode[i].transform.position;//�F�~�`�I��m

                            float G = (nextPosition - neighborPosition).magnitude;//��U�Ӹ`�I��m
                            float H = (neighborPosition - targetPosition).magnitude;//�U�Ӹ`�I��ؼЦ�m
                            float F = G + H;//�Z��

                            if (F < bestDistance)
                            {
                                isHaveBestNodeForNext = true;//�O�_����񪺸`�I
                                bestDistance = F;//�̨ζZ��
                                bestNeighborForNext = i;//�̪񪺾F�~�s��                                
                            }
                        }                               

                        if (isHaveBestNodeForNext)//�O�_����񪺸`�I
                        {
                            //�P�_�P�ؼи��|�O�_����ê��
                            if (!Physics.Linecast(node.neighborNode[bestNeighborForNext].transform.position, targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
                            {
                                pathNodesList.Add(targetPosition);//�����ؼ��I
                                return pathNodesList;//�^�ǩҦ��������|�I
                            }

                            //�P�_�O�_���ؼ��I
                            if (node.neighborNode[bestNeighborForNext].transform.position == targetPosition)
                            {
                                pathNodesList.Add(targetPosition);//�����ؼ��I
                                return pathNodesList;//�^�ǩҦ��������|�I
                            }

                            node = node.neighborNode[bestNeighborForNext];
                            node.nodeState = NodePath.NodeState.����;//�`�I���A
                            closeNodeList.Add(node);//�����w�������`�I
                            pathNodesList.Add(node.transform.position);//�����`�I
                            pathNodesList.Add(targetPosition);//�����ؼ��I
                        }
                        else
                        {
                            //�P�_�P�ؼи��|�O�_����ê��
                            if (Physics.Linecast(node.transform.position, targetPosition, 1 << LayerMask.NameToLayer("StageObject")))
                            {                                
                                continue;
                            }

                            pathNodesList.Add(targetPosition);//�����ؼ��I
                            return pathNodesList;//�^�ǩҦ��������|�I

                        }
                    }
                }
                else
                {
                    pathNodesList.Add(targetPosition);//�����ؼ��I
                    return pathNodesList;//�^�ǩҦ��������|�I
                } 
                pathNodesList.Add(targetPosition);//�����ؼ��I
                return pathNodesList;//�^�ǩҦ��������|�I
            }
            #endregion

            node = node.neighborNode[bestNeighbor];
            node.nodeState = NodePath.NodeState.����;//�`�I���A
            closeNodeList.Add(node);//�����w�������`�I
            pathNodesList.Add(node.transform.position);//��l���|�I

            //�P�_�O�_���ؼ��I
            if (node.transform.position == targetPosition)
            {
                pathNodesList.Add(targetPosition);//�����ؼ��I
                return pathNodesList;//�^�ǩҦ��������|�I
            }
        }
        #endregion

        pathNodesList.Add(targetPosition);//�����ؼ��I
        return pathNodesList;//�^�ǩҦ��������|�I
    }   

    /// <summary>
    /// ����_�I�F�~�`�I
    /// </summary>
    /// <param name="node">�n������`�I</param>
    /// <param name="targetPosition">�ؼЦ�m</param>
    /// <param name="targetPosition">�_�I��m</param>
    /// <param name="distance">�ثe�̪�Z��</param>
    NodePath OnCompareStartNeighborNode(NodePath node, Vector3 targetPosition, Vector3 startPoint, float distance)
    {
        NodePath compareNode = node;
        //float bestDistance = (compareNode.transform.position - targetPosition).magnitude;//�ثe�`�I����I�Z��
        float bestDistance = distance;
        for (int i = 0; i < compareNode.neighborNode.Length; i++)
        {
            if (compareNode.neighborNode[i].nodeState == NodePath.NodeState.����) continue;
            //����ê�����L
            if (Physics.Linecast(startPoint, compareNode.neighborNode[i].transform.position, 1 << LayerMask.NameToLayer("StageObject")))
            {
                continue;
            }

            float neighborDistance = (compareNode.neighborNode[i].transform.position - targetPosition).magnitude;

            /*if (neighborDistance < bestDistance)
            {
                bestDistance = neighborDistance;
                compareNode = compareNode.neighborNode[i];
                Debug.LogError("HaveClose:" + compareNode.transform.name);
            }*/
            Vector3 nextPosition = compareNode.transform.position;//�U�Ӹ`�I��m
            Vector3 neighborPosition = compareNode.neighborNode[i].transform.position;//�F�~�`�I��m

            float G = (nextPosition - neighborPosition).magnitude;//��U�Ӹ`�I��m
            float H = (neighborPosition - targetPosition).magnitude;//�U�Ӹ`�I��ؼЦ�m
            float F = G + H;//�Z��

            if (F < bestDistance)
            {
                bestDistance = F;//�̨ζZ��
                compareNode = compareNode.neighborNode[i];//��s�̪�`�I
                Debug.LogError("HaveClose:"+compareNode.transform.name);
            }
        }
        
        return compareNode;
    }
}
