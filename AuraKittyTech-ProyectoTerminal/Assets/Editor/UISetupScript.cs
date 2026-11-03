using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.InputSystem.UI;

public class UISetupScript
{
    public static void Execute()
    {
        // 1. Clean up existing
        GameObject existing = GameObject.Find("ModelSelectionCanvas");
        if (existing != null) Object.DestroyImmediate(existing);

        // 2. Create Canvas
        GameObject canvasGO = new GameObject("ModelSelectionCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // 3. Create EventSystem
        GameObject es = GameObject.Find("EventSystem");
        if (es == null) es = new GameObject("EventSystem");
        if (es.GetComponent<UnityEngine.EventSystems.EventSystem>() == null) es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        
        var oldModule = es.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        if (oldModule != null) Object.DestroyImmediate(oldModule);
        if (es.GetComponent<InputSystemUIInputModule>() == null) es.AddComponent<InputSystemUIInputModule>();

        // 4. Main Panel
        GameObject panelGO = CreateUIObject("MainPanel", canvasGO.transform);
        Image panelImg = panelGO.AddComponent<Image>();
        panelImg.color = new Color(0.15f, 0.15f, 0.15f, 0.95f);
        RectTransform panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.1f, 0.1f);
        panelRect.anchorMax = new Vector2(0.9f, 0.9f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        // 5. Header (Tabs)
        GameObject headerGO = CreateUIObject("Header", panelGO.transform);
        RectTransform headerRect = headerGO.GetComponent<RectTransform>();
        headerRect.anchorMin = new Vector2(0, 0.8f);
        headerRect.anchorMax = new Vector2(1, 1);
        headerRect.offsetMin = Vector2.zero;
        headerRect.offsetMax = Vector2.zero;

        GameObject titleGO = CreateText("TabTitle", headerGO.transform, "Modelos Escaneados", 28);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.6f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        GameObject tabGroup = CreateUIObject("TabGroup", headerGO.transform);
        RectTransform tgRect = tabGroup.GetComponent<RectTransform>();
        tgRect.anchorMin = new Vector2(0, 0);
        tgRect.anchorMax = new Vector2(1, 0.5f);
        tgRect.offsetMin = new Vector2(20, 0);
        tgRect.offsetMax = new Vector2(-20, 0);
        
        HorizontalLayoutGroup hlg = tabGroup.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 20;
        hlg.childControlWidth = true;
        hlg.childForceExpandWidth = true;

        GameObject tab1 = CreateButton("Tab_Escaneados", tabGroup.transform, "Escaneados");
        GameObject tab2 = CreateButton("Tab_Realizados", tabGroup.transform, "Realizados");

        // 6. Scroll View
        GameObject scrollGO = CreateUIObject("ModelScrollView", panelGO.transform);
        ScrollRect scrollRect = scrollGO.AddComponent<ScrollRect>();
        RectTransform scrollRT = scrollGO.GetComponent<RectTransform>();
        scrollRT.anchorMin = new Vector2(0.05f, 0.15f);
        scrollRT.anchorMax = new Vector2(0.95f, 0.75f);
        scrollRT.offsetMin = Vector2.zero;
        scrollRT.offsetMax = Vector2.zero;
        scrollGO.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.05f, 0.5f);
        
        GameObject viewportGO = CreateUIObject("Viewport", scrollGO.transform);
        viewportGO.AddComponent<Image>(); 
        viewportGO.AddComponent<Mask>().showMaskGraphic = false;
        RectTransform viewportRT = viewportGO.GetComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = Vector2.zero;
        viewportRT.offsetMax = Vector2.zero;
        scrollRect.viewport = viewportRT;

        GameObject contentGO = CreateUIObject("Content", viewportGO.transform);
        RectTransform contentRT = contentGO.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 1);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.pivot = new Vector2(0.5f, 1);
        contentRT.offsetMin = Vector2.zero;
        contentRT.offsetMax = Vector2.zero;
        contentRT.sizeDelta = new Vector2(0, 0);
        scrollRect.content = contentRT;

        VerticalLayoutGroup vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.spacing = 10;
        vlg.padding = new RectOffset(10, 10, 10, 10);
        
        contentGO.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // 7. List Item Template
        GameObject itemTemplate = CreateUIObject("ModelItemTemplate", canvasGO.transform);
        RectTransform itemRT = itemTemplate.GetComponent<RectTransform>();
        itemRT.sizeDelta = new Vector2(0, 80);
        itemTemplate.AddComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        
        // ADDED FIX: LayoutElement to prevent overlapping
        LayoutElement le = itemTemplate.AddComponent<LayoutElement>();
        le.preferredHeight = 80;
        le.minHeight = 80;
        
        GameObject itemText = CreateText("ItemInfo", itemTemplate.transform, "Nombre del modelo por Autor", 18);
        RectTransform itemTextRT = itemText.GetComponent<RectTransform>();
        itemTextRT.anchorMin = Vector2.zero;
        itemTextRT.anchorMax = Vector2.one;
        itemTextRT.offsetMin = new Vector2(100, 0); 
        itemTextRT.offsetMax = new Vector2(-10, 0);
        itemText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        GameObject itemPreview = CreateUIObject("Preview", itemTemplate.transform);
        RectTransform previewRT = itemPreview.GetComponent<RectTransform>();
        previewRT.anchorMin = new Vector2(0, 0.5f);
        previewRT.anchorMax = new Vector2(0, 0.5f);
        previewRT.anchoredPosition = new Vector2(50, 0);
        previewRT.sizeDelta = new Vector2(70, 70);
        itemPreview.AddComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1);

        // 8. Footer Buttons
        GameObject footerGO = CreateUIObject("Footer", panelGO.transform);
        RectTransform footerRect = footerGO.GetComponent<RectTransform>();
        footerRect.anchorMin = new Vector2(0, 0);
        footerRect.anchorMax = new Vector2(1, 0.15f);
        footerRect.offsetMin = Vector2.zero;
        footerRect.offsetMax = Vector2.zero;

        HorizontalLayoutGroup fhlg = footerGO.AddComponent<HorizontalLayoutGroup>();
        fhlg.spacing = 50;
        fhlg.padding = new RectOffset(100, 100, 10, 10);
        fhlg.childControlWidth = true;
        fhlg.childForceExpandWidth = true;

        GameObject btnLoad = CreateButton("Btn_Load", footerGO.transform, "Cargar Modelo");
        GameObject btnSave = CreateButton("Btn_Save", footerGO.transform, "Guardar Actual");

        // 9. Attach Manager and assign references
        ModelUIManager manager = canvasGO.AddComponent<ModelUIManager>();
        manager.listItemPrefab = itemTemplate;
        manager.listContent = contentRT;
        manager.tabTitleText = titleGO.GetComponent<TextMeshProUGUI>();
        manager.btnEscaneados = tab1.GetComponent<Button>();
        manager.btnRealizados = tab2.GetComponent<Button>();
        manager.btnLoad = btnLoad.GetComponent<Button>();
        manager.btnSave = btnSave.GetComponent<Button>();

        itemTemplate.SetActive(false);
        
        Selection.activeGameObject = canvasGO;
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("UI Re-Setup complete with Layout Fix.");
    }

    static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<RectTransform>();
        if (parent != null) go.transform.SetParent(parent, false);
        return go;
    }

    static GameObject CreateText(string name, Transform parent, string content, int fontSize)
    {
        GameObject go = CreateUIObject(name, parent);
        TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        return go;
    }

    static GameObject CreateButton(string name, Transform parent, string label)
    {
        GameObject go = CreateUIObject(name, parent);
        Image img = go.AddComponent<Image>();
        img.color = new Color(0.3f, 0.3f, 0.3f, 1);
        Button btn = go.AddComponent<Button>();
        
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(160, 45);

        GameObject textGO = CreateText("Label", go.transform, label, 18);
        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
        
        return go;
    }
}
