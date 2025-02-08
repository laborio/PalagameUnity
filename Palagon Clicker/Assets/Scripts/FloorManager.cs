using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloorManager : MonoBehaviour
{
    public int unlockLevel; // The level required to unlock this floor
    public GameObject floorVisual; // The container holding floor visuals
    public Button unlockButton; // Button to unlock the floor
    public TMP_Text unlockText; // Text displaying the unlock status
    public Transform spawnPointsContainer; // Parent of the spawn points

    public bool isUnlocked = false; // Tracks whether this floor is unlocked
    private SpawnPoint[] spawnPoints; // Cached spawn points on this floor

    private void Awake()
    {
        // Cache the spawn points from the container
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
    }

private void Start()
{
    // Initialize the unlock button and text
    UpdateUnlockStatus();

    // Add listener to the unlock button
    if (unlockButton != null)
    {
        unlockButton.onClick.AddListener(UnlockFloor);
    }

    // Only hide spawn points if the floor is locked
    if (!isUnlocked)
    {
        UpdateSpawnPointVisibility(false); // Hide spawn points only for locked floors
    }
    else
    {
        UpdateSpawnPointVisibility(true); // Keep spawn points visible for unlocked floors
    }
}


    private void Update()
    {
        // Check if the floor can be unlocked
        if (!isUnlocked && GameManager.Instance.level >= unlockLevel)
        {
            UpdateUnlockStatus();
        }
    }

    /// <summary>
    /// Updates the unlock button and text based on the player's level.
    /// </summary>
    private void UpdateUnlockStatus()
    {
        if (GameManager.Instance.level >= unlockLevel)
        {
            unlockText.text = "Tap to Unlock";
            unlockButton.interactable = true; // Enable the unlock button
        }
        else
        {
            unlockText.text = $"Unlock at Level {unlockLevel}";
            unlockButton.interactable = false; // Disable the unlock button
        }
    }

    /// <summary>
    /// Unlocks the floor and enables its visuals and spawn points.
    /// </summary>
    public void UnlockFloor()
    {
        if (GameManager.Instance.level >= unlockLevel)
        {   
            isUnlocked = true;
            floorVisual.SetActive(true); // Show the floor visuals
            unlockButton.gameObject.SetActive(false); // Hide the unlock button
            Debug.Log($"Floor {name} unlocked!");
        }
    }

    /// <summary>
/// Force unlocks the floor, bypassing the usual unlock logic.
/// </summary>
public void ForceUnlock()
{
    isUnlocked = true;
    floorVisual.SetActive(true); // Show the floor visuals
    if (unlockButton != null)
    {
        unlockButton.gameObject.SetActive(false); // Hide the unlock button
    }

    Debug.Log($"Floor {name} force unlocked.");
}


    /// <summary>
    /// Toggles the visibility of the spawn points on this floor.
    /// </summary>
    /// <param name="visible">Whether the spawn points should be visible.</param>
    public void UpdateSpawnPointVisibility(bool visible)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                spawnPoint.ShowPlaceHolder(visible && isUnlocked); // Only show if unlocked
            }
        }
    }

    /// <summary>
    /// Checks if the floor is unlocked.
    /// </summary>
    public bool IsUnlocked => isUnlocked;
}
