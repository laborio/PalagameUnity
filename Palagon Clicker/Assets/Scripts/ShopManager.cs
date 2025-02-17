using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public GameObject shopPanel;
    public Transform palagonButtonContainer;
    public GameObject palagonButtonPrefab;
    private List<Palagon> shopPalagons = new List<Palagon>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        GenerateShopOptions();
    }

    private void GenerateShopOptions()
{
    foreach (Transform child in palagonButtonContainer)
    {
        Destroy(child.gameObject);
    }

    shopPalagons.Clear();

    List<Palagon> allPalagons = PalagonDatabase.Instance.palagons;
    List<Palagon> filteredPalagons = new List<Palagon>();

    // Define shop odds based on level (TFT-like)
    foreach (Palagon palagon in allPalagons)
    {
        int chance = 0;
        if (palagon.cost == 1) chance = 50 - (GameManager.Instance.playerLevel * 2);
        if (palagon.cost == 2) chance = 30 - (GameManager.Instance.playerLevel);
        if (palagon.cost == 3) chance = 15;
        if (palagon.cost == 4) chance = Mathf.Max(4, GameManager.Instance.playerLevel - 5);
        if (palagon.cost == 5) chance = Mathf.Max(1, GameManager.Instance.playerLevel - 7);

        for (int i = 0; i < chance; i++)
        {
            filteredPalagons.Add(palagon);
        }
    }

    while (shopPalagons.Count < 5)
    {
        Palagon randomPalagon = filteredPalagons[Random.Range(0, filteredPalagons.Count)];
        if (!shopPalagons.Contains(randomPalagon))
        {
            shopPalagons.Add(randomPalagon);
        }
    }

    foreach (Palagon palagon in shopPalagons)
    {
        GameObject buttonObj = Instantiate(palagonButtonPrefab, palagonButtonContainer);
        buttonObj.GetComponentInChildren<TMP_Text>().text = palagon.palagonName + " (" + palagon.cost + "g)";
        buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectPalagon(palagon));
    }
}


   public void SelectPalagon(Palagon selectedPalagon)
{
    PlacementManager.Instance.SelectPalagonFromShop(selectedPalagon);
    CloseShop();
}

public void CloseShop()
{
    if (PlacementManager.Instance.selectedPalagonPrefab == null)
    {
        Debug.Log("You must pick a Palagon before closing the shop!");
        return; // ✅ Prevent closing the shop without selecting a Palagon
    }

    shopPanel.SetActive(false);
}
}
