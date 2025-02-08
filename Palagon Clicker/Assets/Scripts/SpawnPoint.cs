using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("The placeholder sprite that indicates where the entity can be placed.")]
    public GameObject spawnPlaceHolder;

    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D
    private bool isOccupied = false; // Tracks if the spawn point is occupied
    private bool isInteractable = false; // Tracks if the spawn point is currently interactable

    private void Awake()
    {
        // Cache the BoxCollider2D component
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    /// <summary>
    /// Checks if the mouse click hits this spawn point and processes it if interactable.
    /// </summary>
    private void CheckClick()
    {
        if (!isInteractable || isOccupied) return; // Ignore clicks if not interactable or occupied

        // Convert mouse position to world point
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Perform a 2D Raycast to detect colliders
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // Allow placement only if the spawn point is interactable
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.PlaceEntityAtSpawnPoint(this);
                Debug.Log($"SpawnPoint clicked: {name}");
            }
        }
    }

    /// <summary>
    /// Toggles the visibility of the placeholder sprite and the interactability of the spawn point.
    /// </summary>
    public void ShowPlaceHolder(bool show)
    {
        if (spawnPlaceHolder != null)
        {
            spawnPlaceHolder.SetActive(show); // Show or hide the placeholder
        }

        isInteractable = show; // Set interactability based on visibility
        //Debug.Log(show);
        // Enable or disable the collider based on the interactability
        if (boxCollider != null)
        {
            boxCollider.enabled = show;
        }
    }

    /// <summary>
    /// Marks the spawn point as occupied or available.
    /// </summary>
    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
        ShowPlaceHolder(!occupied); // Hide the placeholder if occupied
    }

    /// <summary>
    /// Checks if the spawn point is occupied.
    /// </summary>
    public bool IsOccupied()
    {
        return isOccupied;
    }
}
