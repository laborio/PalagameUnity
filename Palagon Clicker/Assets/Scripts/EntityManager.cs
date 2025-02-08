using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EntityManager : MonoBehaviour
{
    public string entityName = "Default Entity"; // Name of the entity
    public int level = 1; // Current upgrade level
    public float baseGenerationRate = 1f; // Base generation rate per second
    public float generationRate = 0f; // Current generation rate
    public int baseUpgradeCost = 50; // Base cost of upgrades

    public TMP_Text entityLevelText; // Displays the entity's level
    public TMP_Text entityGenerationText; // Displays the value generated per second
    public TMP_Text entityStatusText; // Displays the entity's status (Locked, Available, Unlocked)
    public Button openPanelButton; // Button to open the Global Upgrade Panel

    private SpriteRenderer spriteRenderer; // For visual representation

    public Sprite rank1Sprite; // Sprite for rank 1
    public Sprite rank2Sprite; // Sprite for rank 2
    public Sprite rank3Sprite; // Sprite for rank 3

    private bool isPlaced = false; // Tracks whether the entity has been placed
    private bool isGenerating = false; // Tracks whether the entity is generating

    private void Start()
    {
        // Initialize the button's click listener
        if (openPanelButton != null)
        {
            openPanelButton.onClick.AddListener(OpenUpgradePanel);
        }

        // Initialize UI for this entity
        UpdateLevelText();
    }

    private void OnDestroy()
    {
        // Remove this entity's generation from the total in the GameplayManager
        GameplayManager.Instance.RemoveEntity(this);
    }

    /// <summary>
    /// Initializes the entity with data from EntityData.
    /// </summary>
    public void Initialize(EntityData data)
    {
        entityName = data.entityName;
        baseGenerationRate = data.baseGeneration;
        baseUpgradeCost = data.baseUpgradeCost;
        level = data.currentUpgradeLevel;
        generationRate = CalculatorUtility.GetGenerationRate(baseGenerationRate, level);

        // Assign sprites
        rank1Sprite = data.rank1Sprite;
        rank2Sprite = data.rank2Sprite;
        rank3Sprite = data.rank3Sprite;

        // Update sprite and visuals
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();

        // Update UI elements
        UpdateLevelText();
    }

     private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Click"))
            {
                Debug.Log("Cclick");
                OnEntityClick();
            }
        }
    }

     private void OnEntityClick()
    {
        if (!isPlaced)
        {
            Debug.LogWarning($"{entityName} cannot generate because it is not placed.");
            return;
        }

        int generatedHearts = Mathf.RoundToInt(generationRate);
        GameManager.Instance.AddHearts(generatedHearts);
        Debug.Log($"{entityName} generated {generatedHearts} hearts!");
    }

    /// <summary>
    /// Updates the sprite based on the current upgrade level.
    /// </summary>
    private void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            if (level >= 10 && rank3Sprite != null)
                spriteRenderer.sprite = rank3Sprite;
            else if (level >= 5 && rank2Sprite != null)
                spriteRenderer.sprite = rank2Sprite;
            else if (rank1Sprite != null)
                spriteRenderer.sprite = rank1Sprite;
        }
    }

    /// <summary>
    /// Starts generation after the entity is placed.
    /// </summary>
    public void StartGeneration()
    {
        if (isPlaced && !GameplayManager.Instance.activeEntities.Contains(this))
        {
            GameplayManager.Instance.AddEntity(this);
            Debug.Log($"{entityName} has started generating at {generationRate:F1} per second.");
        }
        else if (!isPlaced)
        {
            Debug.LogWarning($"{entityName} cannot generate because it has not been placed.");
        }
    }

    /// <summary>
    /// Marks the entity as placed and updates the status.
    /// </summary>
    public void PlaceEntity()
    {
        isPlaced = true;
        UpdateLevelText();
        Debug.Log($"{entityName} has been placed.");
    }

    /// <summary>
    /// Opens the Global Upgrade Panel for this entity.
    /// </summary>
    private void OpenUpgradePanel()
    {
        if (GlobalUpgradePanel.Instance != null)
        {
            GlobalUpgradePanel.Instance.OpenPanel(this);
        }
        else
        {
            Debug.LogWarning("GlobalUpgradePanel instance is not available!");
        }
    }

    public float GetGenerationRate()
    {
        return generationRate;
    }

    public void Upgrade()
    {
        int upgradeCost = CalculatorUtility.GetUpgradeCost(baseUpgradeCost, level);

        // Ensure GameManager has enough hearts
        if (GameManager.Instance.hearts >= upgradeCost)
        {
            GameManager.Instance.RemoveHearts(upgradeCost); // Deduct hearts
            level++; // Increment entity level
            generationRate = CalculatorUtility.GetGenerationRate(baseGenerationRate, level);

            // Update sprite based on new level
            UpdateSprite();

            // Update the associated UI
            UpdateLevelText();
            GlobalUpgradePanel.Instance.UpdatePanelUI(this); // If the panel is open
            GameplayManager.Instance.RecalculateTotalGeneration();

            // Update the Entity List Panel
            UIManager.Instance.UpdateEntityList(
                GameplayManager.Instance.activeEntities,
                GameplayManager.Instance.allEntities
            );

            Debug.Log($"{entityName} upgraded to level {level}!");
        }
        else
        {
            Debug.Log("Not enough hearts to upgrade!");
        }
    }

    public int GetUpgradeCost()
    {
        return CalculatorUtility.GetUpgradeCost(baseUpgradeCost, level);
    }

    private void UpdateLevelText()
    {
        if (entityLevelText != null)
        {
            entityLevelText.text = $"Lvl {level}";
        }

        if (entityGenerationText != null)
        {
            entityGenerationText.text = $"{generationRate:F1}/s";
        }

        if (entityStatusText != null)
        {
            entityStatusText.text = isPlaced ? "Unlocked" : "Place to Start";
        }
    }
}
