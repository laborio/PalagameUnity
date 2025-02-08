using UnityEngine;

public class CalculatorUtility : MonoBehaviour
{
    // 1. Generation Per Upgrade Level
    public static float GetGenerationRate(float baseRate, int upgradeLevel)
    {
        // Generation increases exponentially with upgrades.
        float growthFactor = 1.15f; // Exponential growth factor for generation.
        return baseRate * Mathf.Pow(growthFactor, upgradeLevel - 1);
    }

    // 2. Upgrade Cost Scaling
    public static int GetUpgradeCost(int baseCost, int upgradeLevel)
    {
        // Steeper exponential cost with additional scaling based on level.
        float costGrowthFactor = 1.25f;
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costGrowthFactor, upgradeLevel - 1) * upgradeLevel);
    }



    // 3. XP Progression Per Level
    public static int GetXPForNextLevel(int baseXP, int level)
    {
        // XP requirement grows polynomially, with increasing difficulty over time.
        float xpGrowthFactor = 1.5f; // Polynomial growth factor for XP.
        return Mathf.RoundToInt(baseXP * Mathf.Pow(level, xpGrowthFactor));
    }

    // 4. Idle Rewards Calculation
    public static int GetIdleRewards(float elapsedTime, float totalGenerationRate)
    {
        // Calculate rewards based on elapsed time and total generation rate.
        return Mathf.RoundToInt(totalGenerationRate * elapsedTime);
    }

    // 5. Passive Generation
    public static float GetPassiveGeneration(float totalGenerationRate)
    {
        // Return passive generation (same as total rate for now, can add modifiers if needed).
        return totalGenerationRate;
    }
}
