using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �p�a�Ϫ���p�I�I
/// </summary>
public class MiniMapPoint : MonoBehaviour
{
    public Material pointMaterial;

    void Start()
    {
        //�������I���ؤj�p
        float sizeX = transform.parent.GetComponent<BoxCollider>().size.x;
        float sizeY = transform.parent.GetComponent<BoxCollider>().size.y;

        //�]�w�j�p/��m/����
        transform.localScale = new Vector3(sizeX, sizeY, 1);
        transform.position = transform.parent.position;

        //�]�w����
        GetComponent<Renderer>().material = pointMaterial;
    }   
}
