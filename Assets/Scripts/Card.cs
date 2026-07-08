using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardID;  // 記錄這張牌對應哪個水果圖案 (0, 1, 2...)
    public GameObject frontImage, backImage; 
    private GameManager gm;    // 綁定 GameManager，讓卡片可以溝通

    void Start()
    {
        // 被點到的時候去執行 OnCardClicked
        GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }

    public void SetupCard(int id, GameManager manager, Sprite faceSprite)
    {
        cardID = id;
        gm = manager;
    
        // 檢查一下有沒有成功收到圖片
        if (faceSprite == null)
        {
            Debug.LogError("卡片 " + id + " 沒收到圖片！");
        }
        else
        {
            Debug.Log("卡片 " + id + " 順利拿到圖片：" + faceSprite.name);
        }

        // 把正面的圖，換成傳進來的水果圖片
        frontImage.GetComponent<Image>().sprite = faceSprite;
        
        // 遊戲一開始要蓋牌 (關掉正面，打開卡背)
        frontImage.SetActive(false);
        backImage.SetActive(true);
    }
    
    // 點擊卡片
    public void OnCardClicked()
    {
        // 防呆機制
        if (frontImage.activeSelf || gm.isChecking) return;

        // 翻牌 (打開正面，關掉卡背)
        frontImage.SetActive(true);
        backImage.SetActive(false);

        // 呼叫 GameManager
        gm.CardRevealed(this);
    }

    // 把牌蓋回去
    public void CoverCard()
    {
        frontImage.SetActive(false);
        backImage.SetActive(true);
    }
}