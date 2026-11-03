using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FinalSpaceBackgroundSetup
{
    public static void Execute()
    {
        string texturePath = "Assets/Textures/SpaceBackground.png";
        
        // 1. Force Sprite Import
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }
        AssetDatabase.Refresh();

        // 2. Setup UI Background
        GameObject canvasGO = GameObject.Find("ModelSelectionCanvas");
        if (canvasGO == null) return;

        GameObject bgGO = GameObject.Find("ModelSelectionCanvas/SpaceBackgroundUI");
        if (bgGO == null)
        {
            bgGO = new GameObject("SpaceBackgroundUI");
            bgGO.transform.SetParent(canvasGO.transform);
            bgGO.transform.SetAsFirstSibling();
            bgGO.AddComponent<RectTransform>();
        }

        RectTransform rt = bgGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = bgGO.GetComponent<Image>();
        if (img == null) img = bgGO.AddComponent<Image>();
        
        Sprite spaceSprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        if (spaceSprite != null)
        {
            img.sprite = spaceSprite;
            img.color = Color.white;
        }
        else
        {
            img.color = Color.black;
            Debug.LogWarning("Sprite not found, setting black color.");
        }

        // 3. Add some "Nebula Glows" (UI circles with transparency)
        CreateGlow(canvasGO.transform, new Vector2(-400, 200), new Color(0.5f, 0, 1f, 0.2f), new Vector2(800, 800));
        CreateGlow(canvasGO.transform, new Vector2(400, -200), new Color(0, 0.5f, 1f, 0.2f), new Vector2(1000, 1000));

        // 4. Setup Camera and Lighting
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
        }

        // 5. Add Particle System for Stars in the background
        GameObject starsGO = GameObject.Find("SpaceStarsParticles");
        if (starsGO == null)
        {
            starsGO = new GameObject("SpaceStarsParticles");
            ParticleSystem ps = starsGO.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = Color.white;
            main.startSize = 0.05f;
            main.maxParticles = 1000;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = ps.emission;
            emission.rateOverTime = 50;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(20, 20, 1);
            
            starsGO.transform.position = new Vector3(0, 0, 10); // Behind UI
        }

        // Adjust UI Panel transparency to see the background better
        GameObject mainPanel = GameObject.Find("ModelSelectionCanvas/MainPanel");
        if (mainPanel != null)
        {
            Image pImg = mainPanel.GetComponent<Image>();
            pImg.color = new Color(0.05f, 0.05f, 0.1f, 0.5f); // Deep blue tinted dark panel
        }

        Debug.Log("Espacio 'bonito' configurado con éxito.");
    }

    static void CreateGlow(Transform parent, Vector2 pos, Color color, Vector2 size)
    {
        GameObject glow = new GameObject("Glow");
        glow.transform.SetParent(parent);
        glow.transform.SetSiblingIndex(1); // Above background
        RectTransform rt = glow.AddComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        
        Image img = glow.AddComponent<Image>();
        // Using a built-in sprite for the glow effect if available, or just a tinted white square with low alpha
        img.color = color;
    }
}
