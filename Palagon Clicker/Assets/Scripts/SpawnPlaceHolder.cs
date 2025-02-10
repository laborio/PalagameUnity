using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceHolder : MonoBehaviour
{
    public int slotIndex;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click detection
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                PlacementManager.Instance.TryPlacePalagon(slotIndex);
                Debug.Log("SpawnPlaceHolder Clicked via Raycast: Slot " + slotIndex);
            }
        }
    }
}