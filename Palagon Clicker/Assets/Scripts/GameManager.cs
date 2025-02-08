using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int hearts = 0; // Current hearts
    public int totalHeartsGenerated = 0; // Total hearts generated (acts as XP)
    public int level = 1; // Current level

    private float idleTimer = 0f; // Timer for passive generation

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        GenerateIdleHearts();
    }

    public void AddHearts(int amount)
    {
        hearts += amount;
        totalHeartsGenerated += amount; // Increment total hearts generated
        CheckLevelUp(); // Check if level-up is needed
        GameplayManager.Instance.UpdateUI(); // Update the UI
    }

    public void RemoveHearts(int amount)
    {
        if (hearts >= amount)
        {
            hearts -= amount;
            GameplayManager.Instance.UpdateUI(); // Update the UI
        }
        else
        {
            Debug.LogWarning("Not enough hearts!");
        }
    }

    private void GenerateIdleHearts()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= 1f) // Generate passive income every second
        {
            // Calculate idle rewards using the utility method
            int idleRewards = CalculatorUtility.GetIdleRewards(idleTimer, GameplayManager.Instance.totalGenerationRate);
            hearts += idleRewards;
            totalHeartsGenerated += idleRewards;

            idleTimer = 0f;

            CheckLevelUp(); // Check for level-up
            UpdateUI(); // Update the UI
        }
    }

    private void CheckLevelUp()
    {
        int xpForNextLevel = CalculatorUtility.GetXPForNextLevel(100, level);

        if (totalHeartsGenerated >= xpForNextLevel) // Level up if XP threshold is met
        {
            totalHeartsGenerated -= xpForNextLevel;
            level++;
            Debug.Log($"Level Up! Now at level {level}");
        }

        UIManager.Instance.UpdateEntityList(
            GameplayManager.Instance.activeEntities,
            GameplayManager.Instance.allEntities
        );
        UpdateUI(); // Always update UI after XP check
    }

    private void UpdateUI()
    {
        float xpPercentage = GetXPPercentage(); // Calculate XP percentage
        UIManager.Instance.UpdateUI(hearts, xpPercentage, level, GameplayManager.Instance.totalGenerationRate);
    }

    public float GetXPPercentage()
    {
        int xpForNextLevel = CalculatorUtility.GetXPForNextLevel(100, level);
        return (float)totalHeartsGenerated / xpForNextLevel * 100f;
    }

    public void Cheat()
    {
        hearts += 1000000;
    }
}
