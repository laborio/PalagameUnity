using UnityEngine;
using TMPro; // Add this for TextMeshPro support

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign your prefab in the inspector
    public Vector2 positionRangeX = new Vector2(-1f, 1f); // Editable range for X position in inspector
    public Vector2 positionRangeY = new Vector2(0f, 0.7f); // Editable range for Y position in inspector
    public Vector2 scaleRange = new Vector2(0.2f, 0.4f); // Editable range for scale in inspector
    public float destroyTime = 3f; // Editable time before prefab is destroyed

    public TextMeshProUGUI uiCounter; // Assign your TMP UI element in the inspector
    private int spawnCount = 0; // Counter to keep track of spawns

    public void SpawnPrefab()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab to spawn is not assigned!");
            return;
        }

        // Randomize position
        float posX = Random.Range(positionRangeX.x, positionRangeX.y);
        float posY = Random.Range(positionRangeY.x, positionRangeY.y);
        Vector3 spawnPosition = new Vector3(posX, posY, 0);

        // Instantiate prefab
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Randomize scale
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        spawnedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        // Destroy after specified time
        Destroy(spawnedPrefab, destroyTime);

        // Increment and update UI counter
        spawnCount++;
        if (uiCounter != null)
        {
            uiCounter.text = spawnCount.ToString();
        }
    }
}
