using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIBackgroundSetup
{
    public static void Execute()
    {
        GameObject canvasGO = GameObject.Find("ModelSelectionCanvas");
        if (canvasGO == null) return;

        // 1. Find or create Background object
        Transform mainPanel = canvasGO.transform.Find("MainPanel");
        GameObject bgGO = GameObject.Find("ModelSelectionCanvas/SpaceBackgroundUI");
        if (bgGO == null)
        {
            bgGO = new GameObject("SpaceBackgroundUI");
            bgGO.transform.SetParent(canvasGO.transform);
            bgGO.transform.SetAsFirstSibling(); // Send to back
            bgGO.AddComponent<RectTransform>();
        }

        RectTransform rt = bgGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = bgGO.GetComponent<Image>();
        if (img == null) img = bgGO.AddComponent<Image>();

        // 2. Load the generated texture
        string texturePath = "Assets/Textures/SpaceBackground.png";
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        
        if (tex != null)
        {
            // Set texture to Sprite if not already
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
            
            Sprite spaceSprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
            img.sprite = spaceSprite;
            img.color = Color.white;
        }
        else
        {
            img.color = Color.black;
            Debug.LogError("No se pudo cargar la textura en " + texturePath);
        }

        Debug.Log("Fondo UI configurado.");
    }
}
