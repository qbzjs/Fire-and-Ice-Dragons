using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C����Ƥ���(�ƭ�/���|)
/// </summary>
public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;

    [Header("�귽����")]
    public GameData_NumericalValue numericalValue;//�C���ƭ�
    public GameData_LoadPath loadPath;//�C������(���|)

    [Header("�����C�����")]
    public int selectRoleNumber;//��ܪ��}��s��
    public int[] equipBuff;//�˳ƪ�Buff

    void Awake()
    {
        if(gameDataManagement != null)
        {
            Destroy(this);
            return;
        }
        gameDataManagement = this;
        DontDestroyOnLoad(gameObject);

        numericalValue = Resources.Load<ScriptableObject_NumericalValue>("ScriptableObject/NumericalValue").numericalValue;
        loadPath = Resources.Load<ScriptableObject_LoadPath>("ScriptableObject/LoadPath").loadPath;

        //�����C�����
        equipBuff = new int[2] { -1, -1};
    }   
}
