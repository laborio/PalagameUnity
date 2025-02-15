using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public float goldGenerationInterval = 1f;
    public List<Palagon> activePalagons = new List<Palagon>();
    public int xpToNextLevel = 75;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(GenerateResources());
    }

    public void AddPalagon(Palagon palagon)
    {
        activePalagons.Add(palagon);
        UIManager.Instance.UpdateUI();
    }

    private IEnumerator GenerateResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldGenerationInterval);
            GenerateGoldAndXP();
            CheckLevelUp();
            UIManager.Instance.UpdateUI();
        }
    }

    private void GenerateGoldAndXP()
    {
        foreach (var palagon in activePalagons)
        {
            GameManager.Instance.playerGold += palagon.goldGeneration;
            GameManager.Instance.playerXP += palagon.xpGeneration;
        }
    }

   private void CheckLevelUp()
{
    if (GameManager.Instance.playerXP >= xpToNextLevel)
    {
        GameManager.Instance.playerXP -= xpToNextLevel;
        GameManager.Instance.playerLevel++;

        xpToNextLevel = Mathf.RoundToInt(150 * Mathf.Pow(1.8f, GameManager.Instance.playerLevel - 1));

        Debug.Log("Level Up! New Level: " + GameManager.Instance.playerLevel + " | Next XP Required: " + xpToNextLevel);

        // ✅ Find all FloorManagers and unlock floors based on level
        foreach (FloorManager floor in FindObjectsOfType<FloorManager>())
        {
            if (floor.gameObject.name.Contains("Floor2") && GameManager.Instance.playerLevel >= 4)
                floor.UnlockFloor();
            if (floor.gameObject.name.Contains("Floor3") && GameManager.Instance.playerLevel >= 7)
                floor.UnlockFloor();
            if (floor.gameObject.name.Contains("Floor4") && GameManager.Instance.playerLevel >= 10)
                floor.UnlockFloor();
        }

        ShopManager.Instance.OpenShop(); 
    }
}





}
