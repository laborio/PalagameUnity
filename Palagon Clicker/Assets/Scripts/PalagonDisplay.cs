using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PalagonDisplay : MonoBehaviour
{
    [HideInInspector]
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text traitText1;
    public TMP_Text traitText2;
    public TMP_Text goldGenText;
    public TMP_Text xpGenText;
    public GameObject displayPanel;

    [HideInInspector]
    public Palagon palagonData;

    void Start()
    {
        displayPanel.SetActive(false);
    }

    public void InitializePalagon(Palagon palagon)
    {
        palagonData = palagon;
        ShowPalagonInfo();
    }

    public void Update() 
    {
        if (Input.GetMouseButtonDown(0)) // Left-click detection
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ShowPalagonInfo();
            }
        }
    }

    public void ShowPalagonInfo()
    {
        if (palagonData == null)
        {
            Debug.LogError("Palagon data is missing on: " + gameObject.name);
            return;
        }

        displayPanel.SetActive(true);
        nameText.text = palagonData.palagonName;
        costText.text = palagonData.cost + "g";
        traitText1.text = palagonData.primaryTrait;
        traitText2.text = palagonData.secondaryTrait;
        goldGenText.text = palagonData.goldGeneration + " /s gold";
        xpGenText.text = palagonData.xpGeneration + " /s xp";
    }


}
