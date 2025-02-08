using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlobalUpgradePanel : MonoBehaviour
{
    public static GlobalUpgradePanel Instance; // Singleton instance

    public GameObject panel; // The Upgrade Panel GameObject
    public TMP_Text panelNameText; // Displays the entity name
    public TMP_Text panelGenerationText; // Displays the entity's generation rate
    public TMP_Text panelLevelText; // Displays the entity's level
    public TMP_Text panelUpgradeCostText; // Displays the upgrade cost
    public Button upgradeButton; // The upgrade button

    private EntityManager currentEntity; // The currently selected entity

    private void Awake()
    {
        // Ensure a single instance of GlobalUpgradePanel exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPanel(EntityManager entity)
    {
        currentEntity = entity; // Track the selected entity
        UpdatePanelUI(entity); // Update the panel UI
        panel.SetActive(true); // Show the panel
    }

    public void ClosePanel()
    {
        panel.SetActive(false); // Hide the panel
    }

    public void UpgradeEntity()
    {
        if (currentEntity != null)
        {
            currentEntity.Upgrade(); // Call the upgrade method on the selected entity
        }
    }

    public void UpdatePanelUI(EntityManager entity)
    {
        if (entity != null)
        {
            panelNameText.text = entity.entityName;
            panelGenerationText.text = $"{FormatValue(entity.generationRate)}/s";
            panelLevelText.text = $"Level: {entity.level}";
            panelUpgradeCostText.text = $"Cost: {FormatValue(entity.GetUpgradeCost())}";
        }
    }

    /// <summary>
    /// Formats values to short notation (e.g., 10.3k, 1.20m, 1.50b).
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
