using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UISetupHUD
{
    [MenuItem("Tools/Setup HUD")]
    public static void Execute()
    {
        GameObject msCanvasGO = GameObject.Find("ModelSelectionCanvas");
        if (msCanvasGO == null)
        {
            Debug.LogError("ModelSelectionCanvas not found");
            return;
        }

        // 1. Setup Background Canvas
        GameObject bgCanvasGO = GameObject.Find("BackgroundCanvas");
        if (bgCanvasGO == null)
        {
            bgCanvasGO = new GameObject("BackgroundCanvas");
            Canvas bgCanvas = bgCanvasGO.AddComponent<Canvas>();
            bgCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            bgCanvas.sortingOrder = -1;
            bgCanvasGO.AddComponent<CanvasScaler>();
            bgCanvasGO.AddComponent<GraphicRaycaster>();
        }

        // Move Background elements
        Transform bgUI = msCanvasGO.transform.Find("SpaceBackgroundUI");
        if (bgUI != null) bgUI.SetParent(bgCanvasGO.transform, false);
        
        Transform starField = msCanvasGO.transform.Find("StarField");
        if (starField != null) starField.SetParent(bgCanvasGO.transform, false);

        // 2. Setup HUD Canvas
        GameObject hudCanvasGO = GameObject.Find("HUDCanvas");
        if (hudCanvasGO == null)
        {
            hudCanvasGO = new GameObject("HUDCanvas");
            Canvas hudCanvas = hudCanvasGO.AddComponent<Canvas>();
            hudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            hudCanvas.sortingOrder = 10;
            hudCanvasGO.AddComponent<CanvasScaler>();
            hudCanvasGO.AddComponent<GraphicRaycaster>();
        }
        
        UIToggleController toggleController = hudCanvasGO.GetComponent<UIToggleController>();
        if (toggleController == null) toggleController = hudCanvasGO.AddComponent<UIToggleController>();
        toggleController.targetPanel = msCanvasGO;

        // Sprite
        Sprite logoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Recursos/Aura KittyTech.png");

        // 3. Create/Update Toggle Button (Top-Left)
        GameObject btnToggleGO = GameObject.Find("HUDCanvas/Btn_ToggleUI");
        if (btnToggleGO == null)
        {
            btnToggleGO = new GameObject("Btn_ToggleUI");
            btnToggleGO.transform.SetParent(hudCanvasGO.transform, false);
            RectTransform btnRT = btnToggleGO.AddComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0, 1);
            btnRT.anchorMax = new Vector2(0, 1);
            btnRT.pivot = new Vector2(0, 1);
            btnRT.anchoredPosition = new Vector2(20, -20);
            btnRT.sizeDelta = new Vector2(80, 80);
        }

        Image btnImg = btnToggleGO.GetComponent<Image>();
        if (btnImg == null) btnImg = btnToggleGO.AddComponent<Image>();
        btnImg.color = Color.white;
        if (logoSprite != null) btnImg.sprite = logoSprite;

        Button btn = btnToggleGO.GetComponent<Button>();
        if (btn == null) btn = btnToggleGO.AddComponent<Button>();
        
        // Clear existing persistent listeners to avoid duplicates
        while (btn.onClick.GetPersistentEventCount() > 0)
        {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(btn.onClick, 0);
        }
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(btn.onClick, toggleController.TogglePanel);

        // 4. Create/Update Logo (Bottom-Right)
        GameObject logoGO = GameObject.Find("HUDCanvas/AuraLogo_BottomRight");
        if (logoGO == null)
        {
            logoGO = new GameObject("AuraLogo_BottomRight");
            logoGO.transform.SetParent(hudCanvasGO.transform, false);
            RectTransform logoRT = logoGO.AddComponent<RectTransform>();
            logoRT.anchorMin = new Vector2(1, 0);
            logoRT.anchorMax = new Vector2(1, 0);
            logoRT.pivot = new Vector2(1, 0);
            logoRT.anchoredPosition = new Vector2(-20, 20);
            logoRT.sizeDelta = new Vector2(150, 150);
        }

        Image logoImg = logoGO.GetComponent<Image>();
        if (logoImg == null) logoImg = logoGO.AddComponent<Image>();
        if (logoSprite != null) logoImg.sprite = logoSprite;
        logoImg.preserveAspect = true;

        EditorUtility.SetDirty(msCanvasGO);
        if (bgCanvasGO != null) EditorUtility.SetDirty(bgCanvasGO);
        if (hudCanvasGO != null) EditorUtility.SetDirty(hudCanvasGO);
        
        Debug.Log("HUD Setup updated with sprite");
    }
}
