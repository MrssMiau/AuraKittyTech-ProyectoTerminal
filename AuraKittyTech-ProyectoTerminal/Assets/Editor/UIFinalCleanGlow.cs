using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIFinalCleanGlow
{
    public static void Execute()
    {
        GameObject canvasGO = GameObject.Find("ModelSelectionCanvas");
        if (canvasGO == null) return;

        // 1. Thoroughly remove any remaining "Glow" objects that are creating those purple boxes
        GameObject[] allGlows = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var go in allGlows)
        {
            if (go.name == "Glow" && go.transform.IsChildOf(canvasGO.transform))
            {
                Object.DestroyImmediate(go);
            }
        }

        // 2. Adjust Main Panel to be even darker and more transparent to let the nebula show through
        GameObject mainPanel = GameObject.Find("ModelSelectionCanvas/MainPanel");
        if (mainPanel != null)
        {
            Image pImg = mainPanel.GetComponent<Image>();
            pImg.color = new Color(0.01f, 0.01f, 0.03f, 0.6f); // Lower alpha to see nebula
            
            Outline outline = mainPanel.GetComponent<Outline>();
            if (outline != null)
            {
                outline.effectColor = new Color(0.5f, 0.8f, 1f, 0.4f);
                outline.effectDistance = new Vector2(1, 1);
            }
        }

        // 3. Make the scroll area background almost transparent
        GameObject scrollArea = GameObject.Find("ModelSelectionCanvas/MainPanel/ModelScrollView");
        if (scrollArea != null)
        {
            Image sImg = scrollArea.GetComponent<Image>();
            sImg.color = new Color(0, 0, 0, 0.2f);
        }

        Debug.Log("Limpieza de cajas púrpuras y ajuste de transparencia completado.");
    }
}
