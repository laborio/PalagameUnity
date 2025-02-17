using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceHolder : MonoBehaviour
{
    public int floorIndex; // ✅ This should be set in the Inspector per floor
    public int slotIndex; // ✅ Each placeholder should have a unique slot index

    private void OnMouseDown()
    {
        if (PlacementManager.Instance != null)
        {
            PlacementManager.Instance.TryPlacePalagon(floorIndex, slotIndex);
        }
    }
}
