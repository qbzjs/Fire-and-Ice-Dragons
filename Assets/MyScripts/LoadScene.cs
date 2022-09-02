using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ���J����
/// </summary>
public class LoadScene : MonoBehaviour
{
    static LoadScene loadScene;
    public static LoadScene Instance => loadScene;
    GameData_LoadPath loadPath;

    static AsyncOperation ao;//���J����

    static Image background;//���J�I��
    static Image loadBack_Image;//���J�i�ױ�(�I��)
    static Image loadFront_Image;//���J�i�ױ�(�i��)
    static float loadValue;//���J�i��

    private void Awake()
    {
        if (loadScene != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        loadScene = this;
    }

    void Start()
    {
        loadPath = GameDataManagement.Instance.loadPath;

        //���J�I��
        background = ExtensionMethods.FindAnyChild<Image>(transform, "Background_Image");        
        background.enabled = false;

        //���J�i�ױ�(�I��)
        loadBack_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LoadBack_Image");
        loadBack_Image.enabled = false;

        //���J�i�ױ�(�i��)
        loadFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "LoadFront_Image");
        loadFront_Image.enabled = false;

        //StartCoroutine(OnLoadScene("StartScene"));
        SceneManager.LoadScene("StartScene");
    }

    void Update()
    {
        OnLoading();        
    }
        
    /// <summary>
    /// ���J
    /// </summary>
    void OnLoading()
    {       
        if (loadFront_Image.enabled && loadFront_Image.fillAmount < loadValue)
        {
            if (loadValue >= 1) loadValue = 1;            
            loadFront_Image.fillAmount = loadFront_Image.fillAmount + Time.deltaTime;//�i�ױ�            
            if (loadFront_Image.fillAmount >= 1) loadFront_Image.fillAmount = 1;         
        }        
    }

    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="path">�����W��</param>
    /// <returns></returns>
    public IEnumerator OnLoadScene(string scene)
    {
        loadFront_Image.fillAmount = 0;//��l�ƭ�

        //�P�_����(�]�w�I����)
        switch (scene)
        {
            case "StartScene":
                background.sprite = Resources.Load<Sprite>(loadPath.LoadBackground_1);
                break;
            case "GameScene":
                background.sprite = Resources.Load<Sprite>(loadPath.LoadBackground_1);
                break;
        }   

        //�}��UI
        background.enabled = true;
        loadBack_Image.enabled = true;
        loadFront_Image.enabled = true;

        ao = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);//���J����
        //if (ao == null) yield break;//�S�����H�U����

        ao.allowSceneActivation = false;//���J�����۰ʤ���
        
        while (!ao.isDone)//���J������
        {
            loadValue = 0.5f;
            if (ao.progress > 0.89f)//���J�i��
            {
                loadValue = 0.7f;
                yield return new WaitForSeconds(0.3f);
                
                loadValue = 0.85f;
                yield return new WaitForSeconds(0.3f);

                loadValue = 1.0f;
                yield return new WaitForSeconds(0.3f);                                

                //�i�J����
                ao.allowSceneActivation = true;
                StartCoroutine(OnWaitInto());                                            
            }
            yield return 0;
        }
        yield return 0;
    }

    /// <summary>
    /// ���ݶi�J(�קK�ݨ춡�j�����ū�)
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitInto()
    {
        yield return new WaitForSeconds(1.5f);

        //����UI
        background.enabled = false;
        loadBack_Image.enabled = false;
        loadFront_Image.enabled = false;
        yield return 0;
    }
}
