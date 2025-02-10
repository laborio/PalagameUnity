using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerLevel = 1;
    public int playerGold = 0;
    public int playerXP = 0;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("PlayerGold", playerGold);
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        playerGold = PlayerPrefs.GetInt("PlayerGold", 0);
        playerXP = PlayerPrefs.GetInt("PlayerXP", 0);
    }
}