using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
   public static GameplayManager Instance;

    public GameObject entityPrefab; // The prefab for instantiating entities

    public int hearts = 0; // Total hearts
    public int xp = 0; // Total experience points
    public int level = 1; // Player level

    public int heartsPerTap = 1; // Click value (hearts earned per tap)
    public int totalGenerationRate = 0; // Total generation rate of all entities

    public List<EntityManager> activeEntities = new List<EntityManager>(); // List of active entities
    public List<EntityData> allEntities; // List of all entity data

    [Tooltip("Manually reference and order floors here.")]
    public List<FloorManager> allFloors; // Manually referenced list of floor managers

    public EntityData selectedEntityData; // Data for the entity awaiting placement

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (allFloors == null || allFloors.Count == 0)
        {
            Debug.LogError("No floors have been assigned in the GameplayManager.");
        }

        PrepareFirstEntity();
    }

    /// <summary>
    /// Prepares the first entity for placement by unlocking the first floor.
    /// </summary>
  private void PrepareFirstEntity()
{
    if (allEntities.Count > 0)
    {
        selectedEntityData = allEntities[0]; // Automatically select the first entity
    }

    // Automatically unlock the first floor
    if (allFloors.Count > 0)
    {
        FloorManager firstFloor = allFloors[0];
        firstFloor.ForceUnlock(); // Unlock the first floor programmatically

        // Ensure spawn points are visible for the first floor
        firstFloor.UpdateSpawnPointVisibility(true);
    }

    // Simulate the first entity being "available" for placement
    if (selectedEntityData != null)
    {
        Debug.Log($"First entity ({selectedEntityData.entityName}) is ready for placement.");
        PrepareEntityForPlacement(selectedEntityData);
    }
}

public void PrepareEntityForPlacement(EntityData entityData)
{
    if (activeEntities.Exists(e => e.entityName == entityData.entityName))
    {
        Debug.LogWarning($"Entity {entityData.entityName} is already unlocked.");
        return;
    }

    selectedEntityData = entityData;

    // Show placeholders for spawn points on unlocked floors
    foreach (var floor in allFloors)
    {   
        floor.UpdateSpawnPointVisibility(floor.IsUnlocked);
    }

    Debug.Log($"{entityData.entityName} is ready for placement.");
}




    /// <summary>
    /// Places the selected entity at the given spawn point.
    /// </summary>
public void PlaceEntityAtSpawnPoint(SpawnPoint spawnPoint)
{
    if (selectedEntityData == null || spawnPoint == null)
    {
        Debug.LogWarning("No entity data selected or invalid spawn point.");
        return;
    }

    // Instantiate the entity prefab at the spawn point's position
    GameObject newEntity = Instantiate(
        entityPrefab,
        spawnPoint.transform.position,
        Quaternion.identity,
        spawnPoint.transform
    );

    // Get the EntityManager component from the new entity
    EntityManager entityManager = newEntity.GetComponent<EntityManager>();

    // Initialize the entity with its data
    entityManager.Initialize(selectedEntityData);

    // Mark the entity as placed and start generation
    entityManager.PlaceEntity();
    entityManager.StartGeneration();

    // Add the entity to the active entities list
    AddEntity(entityManager);

    // Mark the spawn point as occupied
    spawnPoint.SetOccupied(true);

    // Update the UI
    RecalculateTotalGeneration();
    UIManager.Instance.UpdateEntityList(activeEntities, allEntities);

    // Hide all placeholders
    foreach (var floor in allFloors)
    {
        floor.UpdateSpawnPointVisibility(false); // Hide all spawn points
    }

    // Clear the selected entity data
    selectedEntityData = null;

    Debug.Log($"{entityManager.entityName} has been placed at {spawnPoint.name}.");
}



    /// <summary>
    /// Adds an entity to the active entities list and updates the UI.
    /// </summary>
    public void AddEntity(EntityManager entity)
    {
        if (!activeEntities.Contains(entity))
        {
            activeEntities.Add(entity); // Add the entity to the active list
            RecalculateTotalGeneration(); // Update total generation rate
            UIManager.Instance.UpdateEntityList(activeEntities, allEntities); // Refresh the entity list UI
        }
    }

    /// <summary>
    /// Removes an entity from the active entities list and updates the UI.
    /// </summary>
    public void RemoveEntity(EntityManager entity)
    {
        if (activeEntities.Contains(entity))
        {
            activeEntities.Remove(entity); // Remove the entity from the active list
            RecalculateTotalGeneration(); // Update total generation rate
            UIManager.Instance.UpdateEntityList(activeEntities, allEntities); // Refresh the entity list UI
        }
    }

    /// <summary>
    /// Recalculates the total generation rate.
    /// </summary>
     public void RecalculateTotalGeneration()
    {
        totalGenerationRate = 0;

        foreach (EntityManager entity in activeEntities)
        {
            // Use CalculatorUtility to get generation rate
            totalGenerationRate += Mathf.RoundToInt(CalculatorUtility.GetGenerationRate(entity.baseGenerationRate, entity.level));
        }

        UpdateUI();
    }


    /// <summary>
    /// Updates the UI with the latest data.
    /// </summary>
    public void UpdateUI()
    {
        UIManager.Instance.UpdateUI(
            GameManager.Instance.hearts,
            GetXPPercentage(),
            GameManager.Instance.level,
            totalGenerationRate
        );

        UIManager.Instance.UpdateEntityList(activeEntities, allEntities);
    }

    /// <summary>
    /// Gets the player's XP percentage for the current level.
    /// </summary>
    private float GetXPPercentage()
    {
        int baseXP = 100; // Base XP for level progression
        int xpForNextLevel = CalculatorUtility.GetXPForNextLevel(baseXP, level);
        return (float)xp / xpForNextLevel * 100f;
    }
}
