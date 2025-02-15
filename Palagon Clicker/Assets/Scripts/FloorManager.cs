using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject[] spawnPlaceholders; // Assign placeholders for this floor
    private bool[] isOccupied;

    public CanvasGroup floorLockedUI; // ✅ Each floor has its own lock UI

   void Start()
{
    isOccupied = new bool[spawnPlaceholders.Length];

    // Check if this floor should start as locked
    if (floorLockedUI != null && floorLockedUI.alpha > 0) 
    {
        foreach (var placeholder in spawnPlaceholders)
        {
            placeholder.SetActive(false); // Hide all placeholders if floor is locked
        }
    }

    ShowAvailableSpots(); // Only show spots for unlocked floors
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

   public void UnlockFloor()
{
    // ✅ Unlock placeholders for this floor
    foreach (var placeholder in spawnPlaceholders)
    {
        placeholder.SetActive(true);
    }

    // ✅ Fade out "Locked" UI
    if (floorLockedUI != null)
    {
        StartCoroutine(FadeOutUI(floorLockedUI));
    }

    Debug.Log("Floor unlocked: " + gameObject.name);
}


    private IEnumerator FadeOutUI(CanvasGroup canvasGroup)
    {
        float duration = 1f; // 1 second fade-out
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false); // Hide after fade-out
    }
}
