using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 擊中文字
/// </summary>
public class HitNumber : MonoBehaviour
{
    Canvas canvas_Overlay;
    Text thisText;

    PlayerControl playerControl;

    Transform target;//受傷目標   
    Vector3 startPosition;//初始位置
    float lifeTime;//生存時間
    float speed;//速度
    float addSpeed;//增加的速度
    float randonLoseSpeed;//亂數減少速度
    void Start()
    {
        canvas_Overlay = GameObject.Find("Canvas_Overlay").GetComponent<Canvas>();     
        transform.SetParent(canvas_Overlay.transform);

        lifeTime = 0.55f;//生存時間                
    }

    
    void Update()
    {
        OnHitNumberBehavior();
    }

    /// <summary>
    /// 設定數值
    /// </summary>
    /// <param name="target">受傷目標</param>
    /// <param name="damage">受到傷害</param>
    /// <param name="color">文字顏色</param>
    /// <param name="isCritical">是否爆擊</param>
    public void OnSetValue(Transform target, float damage, Color color, bool isCritical)
    {
        if (thisText == null) thisText = GetComponent<Text>();

        //爆擊字放大
        if (isCritical) thisText.fontSize = 35;
        else thisText.fontSize = 25;

        //符號文字
        string symbolCritical = "";
        string symbol = "";
        if (isCritical) symbolCritical = "爆擊";
        if (color == Color.red) symbol = "-";
        if (color == Color.green) symbol = "+";
        symbol = symbolCritical + symbol;

        //文字
        this.target = target;//受傷目標
        thisText.text = symbol + Mathf.Round(damage).ToString();//受到傷害(四捨五入)        
        thisText.color = color;//文字顏色 
        addSpeed = UnityEngine.Random.Range(8.5f, 12.5f); ;//增加的速度
        randonLoseSpeed = UnityEngine.Random.Range(40.0f, 57.5f);//亂數減少速度   

        playerControl = GameObject.FindObjectOfType<PlayerControl>();

        //與玩家之間有障礙物
        if (Physics.Linecast(target.position + Vector3.up * 1, playerControl.transform.position + Vector3.up * 1, 1 << LayerMask.NameToLayer("StageObject")))
        {
            thisText.enabled = false;
        }
    }
    
    /// <summary>
    /// 擊中文字行為
    /// </summary>
    void OnHitNumberBehavior()
    {
        if (target == null) return;

        //超過距離不顯示        
        if((target.position - Camera.main.transform.position).magnitude > 40) Destroy(gameObject);

        if (addSpeed > 0)
        {
            addSpeed -= randonLoseSpeed * Time.deltaTime;
            if (addSpeed <= 0) addSpeed = 0;
        }

        speed += addSpeed * Time.deltaTime; 

        //文字移動
        startPosition = target.position + target.transform.up * (1 + speed);
        //文字透明度
        thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, lifeTime + 0.3f);

        Camera camera = canvas_Overlay.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(startPosition);        

        //判斷Canvas的RenderMode
        if (canvas_Overlay.renderMode == RenderMode.ScreenSpaceOverlay || camera == null)
        {
            transform.position = position;
        }
        else
        {
            Vector2 localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, camera, out localPosition);
        }

        //生存時間
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 || position.z < 0)
        {
            Destroy(gameObject);
        }
        
        
        //與玩家之間有障礙物
        if (Physics.Linecast(target.position + Vector3.up * 0.5f, playerControl.transform.position + Vector3.up * 0.5f, 1 << LayerMask.NameToLayer("StageObject")))
        {            
            Destroy(gameObject);
        }

        //遊戲結束
        if (GameSceneUI.Instance.isGameOver) Destroy(gameObject);
    }
}
