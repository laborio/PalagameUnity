using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "IdleGame/EntityData", order = 1)]
public class EntityData : ScriptableObject
{
    [Header("Basic Info")]
    public string entityName;         // Name of the entity
    public float baseGeneration;      // Base generation rate

    [Header("Unlock Requirements")]
    public int levelToUnlock;         // Global player level required to unlock this entity

    [Header("Upgrade Info")]
    public int currentUpgradeLevel = 1; // The current upgrade level of the entity
    public int baseUpgradeCost;

    [Header("Visuals")]
    public Sprite rank1Sprite;        // Visual for rank 1
    public Sprite rank2Sprite;        // Visual for rank 2
    public Sprite rank3Sprite;        // Visual for rank 3
}
