using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;
    public GameObject[] floors; // Array of all floor GameObjects
    public GameObject[][] spawnPlaceholders; // Each floor's spawn placeholders
    public CanvasGroup[] floorLockedUI; // Each floor's lock UI
    private bool[] isUnlocked; // Track unlocked state of each floor
    private bool[][] isOccupied; // Track occupied state for each placeholder

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        int floorCount = floors.Length;
        isUnlocked = new bool[floorCount];
        spawnPlaceholders = new GameObject[floorCount][];
        isOccupied = new bool[floorCount][];

        for (int i = 0; i < floorCount; i++)
        {
            SpawnPlaceHolder[] placeholders = floors[i].GetComponentsInChildren<SpawnPlaceHolder>();
            spawnPlaceholders[i] = new GameObject[placeholders.Length];
            isOccupied[i] = new bool[placeholders.Length];

            for (int j = 0; j < placeholders.Length; j++)
            {
                spawnPlaceholders[i][j] = placeholders[j].gameObject;
                isOccupied[i][j] = false; // Ensure all placeholders are initially unoccupied
            }

            // Ensure the first floor starts unlocked, others stay locked
            if (i == 0)
            {
                isUnlocked[i] = true;
            }
            else if (floorLockedUI[i] != null && floorLockedUI[i].alpha > 0)
            {
                isUnlocked[i] = false;
            }

            // Hide placeholders for locked floors
            foreach (var placeholder in spawnPlaceholders[i])
            {
                placeholder.SetActive(isUnlocked[i]);
            }
        }
    }

  public void ShowAvailableSpots(int floorIndex)
{
    //Debug.Log("Updating available spots for Floor: " + floorIndex);

    for (int j = 0; j < spawnPlaceholders[floorIndex].Length; j++)
    {
        bool shouldBeActive = !isOccupied[floorIndex][j];
        spawnPlaceholders[floorIndex][j].SetActive(shouldBeActive);

      //  Debug.Log($"Placeholder {j} on Floor {floorIndex} Active: {shouldBeActive}");
    }
}



   public GameObject PlacePalagon(int floorIndex, int slotIndex, GameObject palagonPrefab)
{
    if (!isOccupied[floorIndex][slotIndex]) // Ensure slot is free
    {
        GameObject newPalagon = Instantiate(palagonPrefab, spawnPlaceholders[floorIndex][slotIndex].transform.position, Quaternion.identity);
        
        isOccupied[floorIndex][slotIndex] = true; // ✅ Mark slot as occupied BEFORE hiding placeholders

        Debug.Log($"Palagon placed at Floor {floorIndex}, Slot {slotIndex}. Hiding remaining placeholders...");

        // ✅ Ensure all placeholders on this floor get updated
        for (int j = 0; j < isOccupied[floorIndex].Length; j++)
        {
            isOccupied[floorIndex][j] = true; // ✅ Mark all slots as occupied
        }

        spawnPlaceholders[floorIndex][slotIndex].SetActive(false); // ✅ Hide the clicked placeholder
        ShowAvailableSpots(floorIndex); // ✅ Update only the current floor's placeholders

        return newPalagon;
    }
    return null;
}




 public void UnlockFloor(int floorIndex)
{
    if (floorIndex < floors.Length && !isUnlocked[floorIndex])
    {
        isUnlocked[floorIndex] = true;

        Debug.Log("Floor unlocked: " + floorIndex);

        // ✅ Reset occupied status for all placeholders on this floor
        for (int j = 0; j < isOccupied[floorIndex].Length; j++)
        {
            isOccupied[floorIndex][j] = false;
        }

        // ✅ Show available placeholders for the new floor
        ShowAvailableSpots(floorIndex);

        // ✅ Fade out "Locked" UI
        if (floorLockedUI[floorIndex] != null)
        {
            StartCoroutine(FadeOutUI(floorLockedUI[floorIndex]));
        }
    }
}


public bool IsFloorUnlocked(int floorIndex)
{
    return isUnlocked[floorIndex];
}


    private IEnumerator FadeOutUI(CanvasGroup canvasGroup)
    {
        float duration = 1f;
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }
}
