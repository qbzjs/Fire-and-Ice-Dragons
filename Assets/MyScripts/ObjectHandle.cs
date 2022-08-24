using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����B�z
/// </summary>
public class ObjectHandle
{
    static ObjectHandle objectHandle;
    public static ObjectHandle GetObjectHandle => objectHandle;

    List<List<TemporaryObject>> searchGameObject_List = new List<List<TemporaryObject>>();//�����Ҧ��C������(�}��/������)    
    List<GameObject> cerateGameObject_List = new List<GameObject>();//�Ыت���(���s�Ыإ�)

    /// <summary>
    /// �غc�l
    /// </summary>
    public ObjectHandle()
    {
        objectHandle = this;
    }
    
    /// <summary>
    /// �Ыت���
    /// </summary>
    /// <param name="path">���J���|</param>
    /// <returns></returns>
    public int OnCreateObject(string path)
    {                
        TemporaryObject temp = new TemporaryObject();

        //�P�_�O�_���s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect) temp.obj = PhotonConnect.Instance.OnCreateObject(path);
        else temp.obj = GameObject.Instantiate(Resources.Load(path) as GameObject);//���ͪ���

        temp.obj.SetActive(false);//��������

        //�s�U����
        List<TemporaryObject> temp_List = new List<TemporaryObject>();//�{�ɦs��
        temp_List.Add(temp);
        cerateGameObject_List.Add(temp.obj);//�s�񪫥�(���s�Ыإ�)
        searchGameObject_List.Add(temp_List);//�s�񶵥�(�}��/������)

        return searchGameObject_List.Count - 1;//�^�Ǫ���s��
    }

    /// <summary>
    /// �}�Ҫ���
    /// </summary>
    /// <param name="number">����s��</param>
    /// <param name="path">prefab���|</param>
    /// <returns></returns>
    public GameObject OnOpenObject(int number, string path)
    {
        if (number < 0 || number > searchGameObject_List.Count) return null;//���b

        List<TemporaryObject> getGameObject_List = searchGameObject_List[number];//���X����

        for (int i = 0; i < getGameObject_List.Count; i++)
        {
            if(!getGameObject_List[i].obj.activeSelf)//�Y����B���������A
            {
                //�s�u�Ҧ�
                if (GameDataManagement.Instance.isConnect)
                {
                    if (getGameObject_List[i].obj.GetComponent<HitNumber>() == null)
                    {
                        PhotonConnect.Instance.OnSendObjectActive(getGameObject_List[i].obj, true);                        
                    }
                }
                
                getGameObject_List[i].obj.SetActive(true);//�}�Ҫ���
                return getGameObject_List[i].obj;//�^�Ǫ���
            }
        }

        //�W�L�ثe�ƶq
        TemporaryObject temp = new TemporaryObject();//�Ȧs����        
        if (GameDataManagement.Instance.isConnect)//�P�_�O�_���s�u�Ҧ�
        {
            temp.obj = PhotonConnect.Instance.OnCreateObject(path);//�Ыت���
            PhotonConnect.Instance.OnSendObjectActive(temp.obj, true);            
        }
        else
        {
            temp.obj = GameObject.Instantiate(cerateGameObject_List[number]) as GameObject;//�Ыطs����(�ƻs����)               
        }

        temp.obj.SetActive(true);//�}�Ҫ���
        searchGameObject_List[number].Add(temp);//�s�U����
        return temp.obj;//�^�Ƿs����
    }
}

/// <summary>
/// �Ȧs����
/// </summary>
public class TemporaryObject
{
    public GameObject obj;
}
