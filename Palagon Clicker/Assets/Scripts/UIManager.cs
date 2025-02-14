using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text goldText;
    public TMP_Text xpText;
    public TMP_Text levelText;
    public Slider levelProgressBar;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

  public void UpdateUI()
{
    int totalGoldGen = 0;
    int totalXPGen = 0;

    foreach (var palagon in GameplayManager.Instance.activePalagons)
    {
        totalGoldGen += palagon.goldGeneration;
        totalXPGen += palagon.xpGeneration;
    }

    goldText.text = GameManager.Instance.playerGold + "g";
    xpText.text = GameManager.Instance.playerXP + "xp";
    levelText.text = "Lv." + GameManager.Instance.playerLevel;

    // XP required for the *previous* and *next* level
    int previousLevelXP = (GameManager.Instance.playerLevel > 1) 
        ? Mathf.RoundToInt(50 * Mathf.Pow(1.8f, GameManager.Instance.playerLevel - 2)) 
        : 0; // Level 1 starts from 0 XP
    
    int nextLevelXP = Mathf.RoundToInt(50 * Mathf.Pow(1.8f, GameManager.Instance.playerLevel - 1));

    // XP progress between previous and next level
    int xpSinceLastLevel = GameManager.Instance.playerXP - previousLevelXP;
    int xpRequiredForNext = nextLevelXP - previousLevelXP;

    levelProgressBar.value = Mathf.Clamp01((float)xpSinceLastLevel / xpRequiredForNext);
}



}
