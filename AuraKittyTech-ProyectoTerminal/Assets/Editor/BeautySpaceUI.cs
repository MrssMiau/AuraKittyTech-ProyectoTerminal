using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class BeautySpaceUI
{
    public static void Execute()
    {
        string texturePath = "Assets/Textures/SpaceBackground.png";
        
        // 1. Force Sprite Import properly
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
        }

        GameObject canvasGO = GameObject.Find("ModelSelectionCanvas");
        if (canvasGO == null) return;

        // 2. Background Image
        GameObject bgGO = GameObject.Find("ModelSelectionCanvas/SpaceBackgroundUI");
        if (bgGO == null)
        {
            bgGO = new GameObject("SpaceBackgroundUI");
            bgGO.transform.SetParent(canvasGO.transform);
            bgGO.transform.SetAsFirstSibling();
            bgGO.AddComponent<RectTransform>();
        }
        
        Image img = bgGO.GetComponent<Image>();
        if (img == null) img = bgGO.AddComponent<Image>();
        
        // Try loading as Texture2D first if Sprite fails
        Sprite spaceSprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        if (spaceSprite == null)
        {
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (tex != null)
            {
                spaceSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
        }

        if (spaceSprite != null)
        {
            img.sprite = spaceSprite;
            img.color = Color.white;
        }
        else
        {
            img.color = new Color(0.05f, 0.05f, 0.1f, 1f); // Dark space blue
        }

        RectTransform bgRT = bgGO.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        // 3. Add Star field (small white dots)
        GameObject starField = GameObject.Find("ModelSelectionCanvas/StarField");
        if (starField != null) Object.DestroyImmediate(starField);
        
        starField = new GameObject("StarField");
        starField.transform.SetParent(canvasGO.transform);
        starField.transform.SetSiblingIndex(1);
        RectTransform sfRT = starField.AddComponent<RectTransform>();
        sfRT.anchorMin = Vector2.zero;
        sfRT.anchorMax = Vector2.one;
        sfRT.offsetMin = Vector2.zero;
        sfRT.offsetMax = Vector2.zero;

        for (int i = 0; i < 50; i++)
        {
            GameObject star = new GameObject("Star");
            star.transform.SetParent(starField.transform);
            Image sImg = star.AddComponent<Image>();
            sImg.color = new Color(1, 1, 1, Random.Range(0.3f, 0.8f));
            RectTransform sRT = star.GetComponent<RectTransform>();
            sRT.anchoredPosition = new Vector2(Random.Range(-960, 960), Random.Range(-540, 540));
            float s = Random.Range(2, 5);
            sRT.sizeDelta = new Vector2(s, s);
        }

        // 4. Stylish Panels
        GameObject mainPanel = GameObject.Find("ModelSelectionCanvas/MainPanel");
        if (mainPanel != null)
        {
            Image pImg = mainPanel.GetComponent<Image>();
            pImg.color = new Color(0.1f, 0.1f, 0.2f, 0.6f); // Semi-transparent dark blue
            
            // Add a subtle border
            Outline outline = mainPanel.GetComponent<Outline>();
            if (outline == null) outline = mainPanel.AddComponent<Outline>();
            outline.effectColor = new Color(0.3f, 0.5f, 1f, 0.5f);
            outline.effectDistance = new Vector2(2, 2);
        }

        // 5. Update Tab Buttons style
        string[] tabs = { "Tab_Escaneados", "Tab_Realizados" };
        foreach (var t in tabs)
        {
            GameObject tab = GameObject.Find("ModelSelectionCanvas/MainPanel/Header/TabGroup/" + t);
            if (tab != null)
            {
                tab.GetComponent<Image>().color = new Color(0.2f, 0.3f, 0.5f, 0.8f);
            }
        }

        Debug.Log("Espacio mejorado configurado.");
    }
}
