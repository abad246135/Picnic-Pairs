using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab, Pedometer;
    public Transform cardContainer;
    public List<Sprite> cardSprites;

    public int totalPairs = 4;

    private List<int> cardIDs = new List<int>();   

    public bool isChecking = false; // 是否正在比對中 (用來鎖住畫面不能點其他張)
    private Card firstCard;         // 記住翻開的第一張牌
    private Card secondCard;        // 記住翻開的第二張牌
    private int matchedPairs = 0;   // 已經成功配對的組數
    private int clickCount = 0;     // 點擊次數 (計步器)
    private float timer = 0f;       // 遊戲時間
    private bool isGameOver = false; // 遊戲是否結束

    void Start()
    {
        for (int i = 0; i < totalPairs; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
        
        // 遊戲一開始就發牌
        GenerateCards();
    }

    void Update()
    {
        // 遊戲還沒結束前，時間一直跑
        if (!isGameOver)
        {
            timer += Time.deltaTime; 
        }
    }

    void GenerateCards()
    {
        // 1. 洗牌 (把陣列裡的 ID 順序隨機打亂)
        for (int i = 0; i < cardIDs.Count; i++) 
        {
            int temp = cardIDs[i];
            int randomIndex = Random.Range(i, cardIDs.Count);
            cardIDs[i] = cardIDs[randomIndex];
            cardIDs[randomIndex] = temp;
        }

        // 2. 發牌 (根據洗好的 ID 生成卡片放到畫面上)
        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardContainer);
            newCard.name = "Card_ID_" + cardIDs[i]; 

            newCard.GetComponent<Card>().SetupCard(cardIDs[i], this, cardSprites[cardIDs[i]]);
        }
    }

    // 接收卡片傳來的翻牌通知
    public void CardRevealed(Card card)
    {
        clickCount++;
        Pedometer.GetComponent<Text>().text =  clickCount.ToString();

        // 判斷這是翻開的第一張還是第二張
        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            // 兩張都翻開了，進行比對
            StartCoroutine(CheckMatch()); 
        }
    }

    // 比對兩張牌的邏輯 (用 IEnumerator 是為了失敗時可以等 1 秒)
    IEnumerator CheckMatch()
    {
        isChecking = true; // 先鎖住桌面，不讓點第三張

        // 檢查兩張牌的 ID 是不是一樣
        if (firstCard.cardID == secondCard.cardID)
        {
            Debug.Log("配對成功！");
            
            // 成功就關掉按鈕功能，讓它們不能再被點
            firstCard.GetComponent<Button>().interactable = false;
            secondCard.GetComponent<Button>().interactable = false;
            matchedPairs++; // 成功組數 +1
            
            // 這裡不寫 yield return，讓程式瞬間執行完，玩家不用等
        }
        else
        {
            // 配對失敗，停頓 1 秒讓玩家看清楚錯在哪
            yield return new WaitForSeconds(1.0f);    

            Debug.Log("配對失敗，蓋回去");
            firstCard.CoverCard();
            secondCard.CoverCard();
        }

        // 判斷是不是全部都配對完了
        if (matchedPairs == totalPairs)
        {
            isGameOver = true; // 停止計時

            // 把過關的點擊次數和時間存進 PlayerPrefs，帶去結算畫面
            PlayerPrefs.SetInt("FinalClicks", clickCount); 
            PlayerPrefs.SetFloat("FinalTime", timer);      
            SceneManager.LoadScene("Result");
        }

        // 這回合比對完畢，清空紀錄，解鎖桌面讓玩家繼續玩
        firstCard = null;
        secondCard = null;
        isChecking = false; 
    }
}