using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Send : MonoBehaviour
{
    // 綁定結算畫面要顯示的 UI 文字
    public Text nametext, leveltext, frequencytext, timetext;

    // Google 表單的接收網址
    private string BASE_URL ="https://docs.google.com/forms/u/0/d/e/1FAIpQLSegU85eVW-aib4ZbnvxMV1vdO1ekhmef8ff_yOqwG54saQiGA/formResponse";

    void Start()
    {
        // 從 PlayerPrefs 抓出後台的遊戲紀錄
        string pName = PlayerPrefs.GetString("PlayerName", "未填寫");
        string pLevel = PlayerPrefs.GetString("Difficulty", "未知");
        string pFreq = PlayerPrefs.GetInt("FinalClicks", 0).ToString();
        string pTime = PlayerPrefs.GetFloat("FinalTime", 0f).ToString("F2"); // F2(只取到小數點後兩位)

        // 把紀錄更新到畫面的 Text 上
        if (nametext != null) nametext.text = "Name : " + pName;
        if (leveltext != null) leveltext.text = "Level : " + pLevel;
        if (frequencytext != null) frequencytext.text = "Count : " + pFreq;
        if (timetext != null) timetext.text = "Time : " + pTime + ".sec";

        // 啟動協程，把這筆成績丟到後台的 Google 表單
        StartCoroutine(PostData(pName, pLevel, pFreq, pTime));
    }

    // 負責背景上傳資料的協程
    IEnumerator PostData(string name, string level, string frequency, string time)
    {
        // 打包要傳送的表單資料 (對應 Google 表單的欄位 ID)
        WWWForm form = new WWWForm();
        form.AddField("entry.1502447425", name);
        form.AddField("entry.1962811268", level);
        form.AddField("entry.1091603695", frequency);
        form.AddField("entry.1507227175", time);

        // 發送 Post 請求
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL, form))
        {
            yield return www.SendWebRequest(); // 等待伺服器回應

            // 檢查上傳結果
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("資料上傳成功！玩家：" + name + " 難度：" + level);
            }
            else
            {
                // 用 LogError 標示紅字，網路斷線時能立刻發現
                Debug.LogError("資料上傳失敗：" + www.error);
            }
        }
    }
}