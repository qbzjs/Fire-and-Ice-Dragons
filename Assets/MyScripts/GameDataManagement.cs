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

    /// <summary>
    /// �ثe����
    /// </summary>
    public enum Stage {�}�l����,�C������}
    public Stage stage = new Stage();

    [Header("�귽����")]
    public GameData_NumericalValue numericalValue;//�C���ƭ�
    public GameData_LoadPath loadPath;//�C������(���|)

    [Header("�����C�����")]
    public float musicVolume;//���֭��q
    public bool isConnect;//�O�_�s�u
    public bool isNotFirstIntoGame;//�O�_�Ĥ@���i�J�C��
    public int selectRoleNumber;//��ܪ��}��s��
    public int selectLevelNumber;//��ܪ����d�s��
    public int[] equipBuff;//�˳ƪ�Buff
    public int[] allConnectPlayerSelectRole = new int[] { 0, 0, 0, 0};//�Ҧ��s�u���a�ҿ﨤��

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
        musicVolume = 0.3f;//���֭��q
        equipBuff = new int[2] { -1, -1};//�˳ƪ�Buff

        selectLevelNumber = 11;//��ܪ����d�s��
    }   
}
