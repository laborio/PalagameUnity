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
    selectedPalagonPrefab = Resources.Load<GameObject>("Palagons/" + selectedPalagonData.palagonName);
    selectedPalagonPrefab.GetComponent<PalagonDisplay>().InitializePalagon(selectedPalagonData);

    Debug.Log("Selected new Palagon: " + selectedPalagonData.palagonName);

    // ✅ Show available placeholders for all unlocked floors
    for (int i = 0; i < FloorManager.Instance.floors.Length; i++)
    {
        if (FloorManager.Instance.IsFloorUnlocked(i)) // Ensure floor is unlocked
        {
            FloorManager.Instance.ShowAvailableSpots(i);
        }
    }
}


    public void TryPlacePalagon(int floorIndex, int slotIndex)
    {
        if (selectedPalagonPrefab == null)
        {
            Debug.Log("No Palagon selected! Please pick one from the shop first.");
            return;
        }

        GameObject newPalagon = FloorManager.Instance.PlacePalagon(floorIndex, slotIndex, selectedPalagonPrefab);

        if (newPalagon != null)
        {
            PalagonDisplay palagonDisplay = newPalagon.GetComponent<PalagonDisplay>();
            if (palagonDisplay != null)
            {
                GameplayManager.Instance.AddPalagon(palagonDisplay.palagonData);
            }

            // ✅ Hide placeholders after placement
            FloorManager.Instance.ShowAvailableSpots(floorIndex);

            // ✅ Clear selection so the player must select a new Palagon before placing another
            selectedPalagonPrefab = null;
        }
    }
}