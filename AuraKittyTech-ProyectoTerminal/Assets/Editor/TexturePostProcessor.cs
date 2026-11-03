using UnityEngine;
using UnityEditor;

public class TexturePostProcessor
{
    [MenuItem("Tools/Fix Logo Import Settings")]
    public static void FixLogo()
    {
        string path = "Assets/Recursos/Aura KittyTech.png";
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
                Debug.Log("Logo texture type changed to Sprite.");
            }
            else
            {
                Debug.Log("Logo is already a Sprite.");
            }
        }
        else
        {
            Debug.LogError("Logo not found at " + path);
        }
    }
}
