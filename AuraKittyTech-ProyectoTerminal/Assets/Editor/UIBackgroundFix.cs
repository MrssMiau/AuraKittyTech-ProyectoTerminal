using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIBackgroundFix
{
    public static void Execute()
    {
        string texturePath = "Assets/Textures/SpaceBackground.png";
        
        // 1. Force Texture Import Settings to Sprite
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        // 2. Find the Image component and assign the sprite
        GameObject bgGO = GameObject.Find("ModelSelectionCanvas/SpaceBackgroundUI");
        if (bgGO != null)
        {
            Image img = bgGO.GetComponent<Image>();
            Sprite spaceSprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
            if (spaceSprite != null)
            {
                img.sprite = spaceSprite;
                img.color = Color.white;
                Debug.Log("Sprite de espacio asignado exitosamente.");
            }
            else
            {
                Debug.LogError("No se pudo cargar el Sprite después del reimport.");
            }
        }
        
        // 3. Make panels slightly more transparent to see the background
        GameObject mainPanel = GameObject.Find("ModelSelectionCanvas/MainPanel");
        if (mainPanel != null)
        {
            Image pImg = mainPanel.GetComponent<Image>();
            pImg.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        }

        GameObject scrollArea = GameObject.Find("ModelSelectionCanvas/MainPanel/ModelScrollView");
        if (scrollArea != null)
        {
            Image sImg = scrollArea.GetComponent<Image>();
            sImg.color = new Color(0.05f, 0.05f, 0.05f, 0.4f);
        }
    }
}
