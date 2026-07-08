using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    public InputField nameInputField; // 綁定輸入名字的欄位
    
    // 單純切換畫面的按鈕
    public void ToInfo()
    {
        SceneManager.LoadScene("Info");
    }
    public void ToHelp()
    {
        SceneManager.LoadScene("Help");
    }
    public void ToHelp1()
    {
        SceneManager.LoadScene("Help1");
    }
    public void ToMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void ToLevel()
    {
        SceneManager.LoadScene("Level");
    }

    // 選擇難度並進入遊戲的按鈕
    public void ToGame1()
    {
        PlayerPrefs.SetString("Difficulty", "Easy"); // 簡單
        SceneManager.LoadScene("Game1");
    }
    public void ToGame2()
    {
        PlayerPrefs.SetString("Difficulty", "Normal"); // 普通
        SceneManager.LoadScene("Game2");
    }
    public void ToGame3()
    {
        PlayerPrefs.SetString("Difficulty", "Hard"); // 困難
        SceneManager.LoadScene("Game3");
    }

    // 記錄到後台
    public void NameYes()
    {
        // 把輸入的名字存到系統裡 (PlayerPrefs)
        PlayerPrefs.SetString("PlayerName", nameInputField.text);
        
        // 跳轉到選關畫面
        SceneManager.LoadScene("Level");
    }
}