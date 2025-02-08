using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text heartsText; // Displays total hearts
    public TMP_Text xpText; // Displays XP percentage
    public TMP_Text levelText; // Displays player level
    public TMP_Text totalGenerationRateText; // Displays total generation rate

    public GameObject entityPrefab; // Prefab for entity UI
    public GameObject entityListPanel;
    public Transform entityListContentParent; // Parent for the entity list items
    private readonly List<GameObject> entityUIInstances = new List<GameObject>(); // Track instantiated UI elements

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("UIManager is not assigned!");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Updates the core UI elements (hearts, XP, level, generation rate).
    /// </summary>
    public void UpdateUI(int hearts, float xpPercentage, int level, int totalGenerationRate)
    {
        heartsText.text = FormatValue(hearts);
        xpText.text = $"XP: {xpPercentage:F1}%";
        levelText.text = $"Level: {level}";
        totalGenerationRateText.text = $"Rate: {FormatValue(totalGenerationRate)} hearts/sec";
    }

    /// <summary>
    /// Updates the entity list UI to display both active and locked entities.
    /// </summary>
    public void UpdateEntityList(List<EntityManager> activeEntities, List<EntityData> allEntities)
    {
        // Clear existing UI instances
        ClearEntityList();

        // Populate UI with all entities
        foreach (var entityData in allEntities)
        {
            CreateEntityUI(entityData, activeEntities);
        }
    }

    /// <summary>
    /// Clears the entity list UI by destroying all existing instances.
    /// </summary>
    private void ClearEntityList()
    {
        foreach (var uiInstance in entityUIInstances)
        {
            Destroy(uiInstance);
        }
        entityUIInstances.Clear();
    }

    /// <summary>
    /// Creates a UI entry for an entity and populates its data.
    /// </summary>
    private void CreateEntityUI(EntityData entityData, List<EntityManager> activeEntities)
    {
        GameObject entityUI = Instantiate(entityPrefab, entityListContentParent);
        entityUIInstances.Add(entityUI);

        var activeEntity = activeEntities.Find(e => e.entityName == entityData.entityName);

        TMP_Text[] tmpTexts = entityUI.GetComponentsInChildren<TMP_Text>();
        Button statusButton = null;

        foreach (var tmpText in tmpTexts)
        {
            switch (tmpText.name)
            {
                case "EntityNameText":
                    tmpText.text = entityData.entityName;
                    break;
                case "EntityLevelText":
                    tmpText.text = activeEntity != null
                        ? $"Level: {activeEntity.level}"
                        : "Level: 0";
                    break;
                case "EntityGenerationText":
                    tmpText.text = activeEntity != null
                        ? $"{FormatValue(activeEntity.generationRate)}/s"
                        : $"{FormatValue(entityData.baseGeneration)}/s";
                    break;
                case "EntityStatusText":
                    if (activeEntity != null)
                    {
                        tmpText.text = "Unlocked";
                    }
                    else if (GameManager.Instance.level >= entityData.levelToUnlock)
                    {
                        tmpText.text = "Available";
                        statusButton = entityUI.GetComponentInChildren<Button>();
                        if (statusButton != null)
                        {
                            statusButton.onClick.AddListener(() =>
                                UnlockEntity(entityData)); // Assign unlock logic
                        }
                    }
                    else
                    {
                        tmpText.text = $"Locked (Level {entityData.levelToUnlock})";
                    }
                    break;
                default:
                    Debug.LogWarning($"Unhandled TMP_Text element: {tmpText.name}");
                    break;
            }
        }
    }

    /// <summary>
    /// Unlocks an entity for placement if available.
    /// </summary>
    private void UnlockEntity(EntityData entityData)
    {
        Debug.Log($"Unlocking entity: {entityData.entityName}");
        GameplayManager.Instance.PrepareEntityForPlacement(entityData);

        if (entityListPanel != null)
        {
            entityListPanel.SetActive(false);
        }

        // Show spawn point placeholders only for unlocked floors
        foreach (var floor in GameplayManager.Instance.allFloors)
        {
            if (floor.IsUnlocked)
            {
                floor.UpdateSpawnPointVisibility(true);
            }
            else
            {
                floor.UpdateSpawnPointVisibility(false);
            }
        }
    }

    /// <summary>
    /// Formats values to short notation (e.g., 10.3k, 1.00m, 1.00b).
    /// </summary>
    private string FormatValue(float value)
    {
        if (value >= 1_000_000_000) // Billions
            return $"{value / 1_000_000_000:F2}b";
        else if (value >= 1_000_000) // Millions
            return $"{value / 1_000_000:F2}m";
        else if (value >= 10_000) // Thousands
            return $"{value / 1_000:F1}k";
        else
            return value.ToString("F0"); // Default format for smaller values
    }
}
