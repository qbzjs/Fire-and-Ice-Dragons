using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    const float radius = 0.5f;//Gizmos

    [Tooltip("�F�~�`�I")] public NodePath[] neighborNode = new NodePath[] { };
    
    /// <summary>
    /// �`�I���A
    /// </summary>
    public enum NodeState
    {
        �}��,
        ����
    }
    public NodeState nodeState;

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);*/

        for (int i = 0; i < neighborNode.Length; i++)
        {
            Gizmos.color = Color.black;
            
            Gizmos.DrawLine(transform.position, neighborNode[i].transform.position);
        }
    }
}
