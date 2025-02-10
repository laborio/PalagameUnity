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
    levelProgressBar.value = (float)GameManager.Instance.playerXP / (GameManager.Instance.playerLevel * 10);
}

}
