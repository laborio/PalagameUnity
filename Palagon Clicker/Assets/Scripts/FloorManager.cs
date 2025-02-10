using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject[] spawnPlaceholders; // Assign the 3 placeholders in the inspector
    private bool[] isOccupied;

    void Start()
    {
        isOccupied = new bool[spawnPlaceholders.Length];
        ShowAvailableSpots();
    }

    public void ShowAvailableSpots()
    {
        for (int i = 0; i < spawnPlaceholders.Length; i++)
        {
            if (!isOccupied[i])
                spawnPlaceholders[i].SetActive(true);
        }
    }

    public GameObject PlacePalagon(int index, GameObject palagonPrefab)
{
    if (!isOccupied[index]) // Ensure slot is free
    {
        GameObject newPalagon = Instantiate(palagonPrefab, spawnPlaceholders[index].transform.position, Quaternion.identity);
        isOccupied[index] = true;
        spawnPlaceholders[index].SetActive(false);
        return newPalagon; // Return the instantiated Palagon
    }
    return null; // If placement fails, return null
}

}
