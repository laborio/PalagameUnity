using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;
    public GameObject selectedPalagonPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectPalagon(GameObject palagonPrefab)
    {
        selectedPalagonPrefab = palagonPrefab;
    }

    public void SelectPalagonFromShop(Palagon selectedPalagonData)
{
    selectedPalagonPrefab = Resources.Load<GameObject>("Palagons/" + selectedPalagonData.palagonName); // Load prefab dynamically
    selectedPalagonPrefab.GetComponent<PalagonDisplay>().InitializePalagon(selectedPalagonData);
}

  public void TryPlacePalagon(int slotIndex)
{
    if (selectedPalagonPrefab != null)
    {
        FloorManager floorManager = FindObjectOfType<FloorManager>();
        if (floorManager != null)
        {
            GameObject newPalagon = floorManager.PlacePalagon(slotIndex, selectedPalagonPrefab);
            if (newPalagon != null)
            {
                PalagonDisplay palagonDisplay = newPalagon.GetComponent<PalagonDisplay>();
                if (palagonDisplay != null)
                {
                    string cleanName = selectedPalagonPrefab.name.Replace("(Clone)", "").Trim();
                    Palagon originalPalagon = PalagonDatabase.Instance.GetPalagon(cleanName);

                    if (originalPalagon != null)
                    {
                        // Assign a unique instance of the Palagon data
                        Palagon instancePalagon = new Palagon(
                            originalPalagon.palagonName,
                            originalPalagon.cost,
                            originalPalagon.primaryTrait,
                            originalPalagon.secondaryTrait,
                            originalPalagon.goldGeneration,
                            originalPalagon.xpGeneration
                        );

                        palagonDisplay.InitializePalagon(instancePalagon);

                        // ✅ Register the new Palagon for gold and XP generation
                        GameplayManager.Instance.AddPalagon(instancePalagon);
                    }
                    else
                    {
                        Debug.LogError("Palagon data not found for: " + cleanName);
                    }
                }
            }
        }
        selectedPalagonPrefab = null;
    }
}

}
