using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public float goldGenerationInterval = 1f;
    public float xpGenerationInterval = 1f;
    public List<Palagon> activePalagons = new List<Palagon>();

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
        Debug.Log("Added Palagon: " + palagon.palagonName + " | Gold: " + palagon.goldGeneration + " | XP: " + palagon.xpGeneration);
        UIManager.Instance.UpdateUI(); // ✅ Update UI immediately after adding
    }

    private IEnumerator GenerateResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldGenerationInterval);
            GenerateGoldAndXP();
            UIManager.Instance.UpdateUI(); // ✅ Update UI every tick
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
}
