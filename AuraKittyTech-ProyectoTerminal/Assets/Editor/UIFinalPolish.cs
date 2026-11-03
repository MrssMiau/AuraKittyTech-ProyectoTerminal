using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIFinalPolish
{
    public static void Execute()
    {
        GameObject canvasGO = GameObject.Find("ModelSelectionCanvas");
        if (canvasGO == null) return;

        // 1. Remove the square "Glow" objects from previous attempts
        foreach (Transform child in canvasGO.transform)
        {
            if (child.name == "Glow")
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        // 2. Adjust Main Panel to be dark and sleek
        GameObject mainPanel = GameObject.Find("ModelSelectionCanvas/MainPanel");
        if (mainPanel != null)
        {
            Image pImg = mainPanel.GetComponent<Image>();
            pImg.color = new Color(0.02f, 0.02f, 0.05f, 0.85f); // Very dark blue/black
            
            Outline outline = mainPanel.GetComponent<Outline>();
            if (outline == null) outline = mainPanel.AddComponent<Outline>();
            outline.effectColor = new Color(0.4f, 0.6f, 1f, 0.3f);
            outline.effectDistance = new Vector2(1, -1);
        }

        // 3. Adjust Scroll View Background
        GameObject scrollArea = GameObject.Find("ModelSelectionCanvas/MainPanel/ModelScrollView");
        if (scrollArea != null)
        {
            Image sImg = scrollArea.GetComponent<Image>();
            sImg.color = new Color(0, 0, 0, 0.4f);
        }

        // 4. Style Footer Buttons
        GameObject btnLoad = GameObject.Find("ModelSelectionCanvas/MainPanel/Footer/Btn_Load");
        if (btnLoad != null) btnLoad.GetComponent<Image>().color = new Color(0.1f, 0.4f, 0.2f, 0.9f); // Dark Greenish

        GameObject btnSave = GameObject.Find("ModelSelectionCanvas/MainPanel/Footer/Btn_Save");
        if (btnSave != null) btnSave.GetComponent<Image>().color = new Color(0.1f, 0.2f, 0.4f, 0.9f); // Dark Bluish

        // 5. Setup a separate Star Field under the UI but above background
        GameObject starField = GameObject.Find("ModelSelectionCanvas/StarField");
        if (starField != null)
        {
            foreach (Transform star in starField.transform)
            {
                Image sImg = star.GetComponent<Image>();
                // Make them look more like stars (circles if possible, or just tiny dots)
                sImg.color = new Color(1, 1, 1, Random.Range(0.2f, 1.0f));
            }
        }

        Debug.Log("Pulido final de interfaz espacial completado.");
    }
}
